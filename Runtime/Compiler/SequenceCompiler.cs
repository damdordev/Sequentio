// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Compiles sequence definitions into timed plans and executable step players.
    /// </summary>
    public class SequenceCompiler
    {
        private readonly Dictionary<Type, StepCompiler> stepCompilers = new();
        
        /// <summary>
        /// Compiles enabled steps and evaluates their timings using cloned sequence settings and storage.
        /// </summary>
        /// <param name="definition">The definition used to configure playback.</param>
        /// <param name="settings">The playback settings to copy for the sequence.</param>
        /// <param name="modify">An optional callback that modifies cloned storage and settings before step timings are evaluated.</param>
        /// <returns>A pooled plan. Call its Release method when it is no longer needed.</returns>
        public SequencePlan CreateDefinitionPlan(
            ISequenceDefinition definition,
            SequenceSettings settings, 
            Action<VarioStorage, SequenceSettings> modify = null)
        {
            var plan = SequentioPooling.PopSequenceDefinitionPlan();
            plan.Settings = settings.Clone();
            plan.Storage = definition.Storage.Clone();
            modify?.Invoke(plan.Storage, plan.Settings);
           
            CreateStepsDefinition(plan, definition);
            RecalculateStepsTimes(plan);
            CalculateDurationAndInitialTime(plan);
            
            return plan;
        }
        
        /// <summary>
        /// Creates a pooled execution with cloned settings and storage and a player for each successfully compiled step.
        /// </summary>
        /// <param name="sequenceDefinitionPlan">The compiled sequence plan.</param>
        /// <returns>A pooled execution. Call its Release method when it is no longer needed.</returns>
        public SequenceExecution CreateExecution(SequencePlan sequenceDefinitionPlan)
        {
            var playerPlan = SequentioPooling.PopSequenceExecution();
            
            playerPlan.Duration = sequenceDefinitionPlan.Duration;
            playerPlan.Settings = sequenceDefinitionPlan.Settings.Clone();
            playerPlan.Storage = sequenceDefinitionPlan.Storage.Clone();
            
            foreach (var stepDefinitionPlan in sequenceDefinitionPlan.Steps)
            {
                var stepExecution = SequentioPooling.PopStepExecution();
                
                stepExecution.Definition = stepDefinitionPlan.Definition;
                try
                {
                    stepExecution.Player = CreateStepPlayer(stepDefinitionPlan.Definition);
                }
                catch (Exception e)
                {
                    SequentioSettings.NotifyOnStepError(new SequentioStepErrorHandlerData(
                            SequentioStepErrorSource.CreatePlayer,
                            stepDefinitionPlan.Definition,
                            null,
                            e
                        )
                    );
                    stepExecution.Release();
                    continue;
                }

                stepExecution.StartTime = stepDefinitionPlan.StartTime;
                stepExecution.EndTime = stepDefinitionPlan.EndTime;
                stepExecution.Duration = stepDefinitionPlan.Duration;
                stepExecution.Delay = stepDefinitionPlan.Delay;
                stepExecution.InitOnStart = stepDefinitionPlan.InitOnStart;
                stepExecution.RunWithPreviousStep = stepDefinitionPlan.RunWithPreviousStep;

                playerPlan.Steps.Add(stepExecution);
            }

            return playerPlan;
        }
        
        public void RegisterStep<TStepDefinition, TStepPlayer>()
            where TStepDefinition : StepDefinition
            where TStepPlayer : StepPlayer<TStepDefinition>, new()
        {
            stepCompilers[typeof(TStepDefinition)] = new StepCompiler<TStepDefinition, TStepPlayer>();
        }
        
        public StepPlayer CreateStepPlayer(StepDefinition stepDefinition)
        {
            return !stepCompilers.TryGetValue(stepDefinition.GetType(), out var stepCompiler) 
                ? throw new ArgumentException($"Invalid step definition type: {stepDefinition.GetType().FullName}") 
                : stepCompiler.Compile(stepDefinition);
        }
        
         private static void CreateStepsDefinition(SequencePlan plan, ISequenceDefinition definition)
         {
             for (var index = 0; index < definition.Steps.Count; index++)
             {
                 var stepDefinition = definition.Steps[index];
                 if (!stepDefinition.Enabled.Evaluate(plan.Storage)) continue;

                 var stepPlan = SequentioPooling.PopStepDefinitionPlan();
                 stepPlan.Definition = stepDefinition;
                 stepPlan.RunWithPreviousStep = stepDefinition.RunWithPreviousStep;
                 stepPlan.InitOnStart = stepDefinition.InitOnStart;
                 stepPlan.Delay = stepDefinition.Delay.Evaluate(plan.Storage);
                 try
                 {
                     stepPlan.Duration = stepDefinition.CalculateDuration(plan.Storage);
                 }
                 catch (Exception e)
                 {
                     SequentioSettings.NotifyOnStepError(new SequentioStepErrorHandlerData(
                             SequentioStepErrorSource.CalculateDuration,
                             stepDefinition,
                             null,
                             e
                         )
                     );
                     continue;
                 }

                 stepPlan.OriginalIndex = index;

                 plan.Steps.Add(stepPlan);
             }
         }

         private static void RecalculateStepsTimes(SequencePlan plan)
         {
             var groupStartTime = 0f;
             var groupDuration = 0f;
             var groupSize = 0;

             foreach (var stepPlan in plan.Steps)
             {
                 if (stepPlan.RunWithPreviousStep)
                 {
                     groupDuration = Mathf.Max(groupDuration, stepPlan.Duration + stepPlan.Delay);
                 }
                 else
                 {
                     groupSize = 0;
                     groupStartTime += groupDuration;
                     groupDuration = stepPlan.Duration + stepPlan.Delay;
                 }

                 stepPlan.StartTime = groupStartTime + stepPlan.Delay;
                 stepPlan.EndTime = groupStartTime + stepPlan.Duration + stepPlan.Delay;
                 stepPlan.IndexInGroup = groupSize++;
             }
         }
         
         private static void CalculateDurationAndInitialTime(SequencePlan plan)
         {
             plan.Duration = 0f;
             foreach (var stepData in plan.Steps)
             {
                 plan.Duration = Mathf.Max(plan.Duration, stepData.EndTime);
             }
         }

    }
    

}
