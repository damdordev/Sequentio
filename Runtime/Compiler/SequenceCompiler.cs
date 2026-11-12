// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public static class SequenceCompiler
    {
        private static readonly Dictionary<Type, StepCompiler> stepCompilers = new();
        
        public static SequencePlan CreateDefinitionPlan(
            ISequenceDefinition definition,
            SequenceSettings settings, 
            bool runtime, 
            Action<VarioStorage, SequenceSettings> modify = null)
        {
            var plan = SequentioPooling.PopSequenceDefinitionPlan();
            plan.Settings = settings.Clone();
            plan.Storage = definition.Storage.Clone();
            modify?.Invoke(plan.Storage, plan.Settings);
           
            CreateStepsDefinition(plan, definition, runtime);
            RecalculateStepsTimes(plan);
            CalculateDurationAndInitialTime(plan);
            
            return plan;
        }
        
        public static SequenceExecution CreateExecution(SequencePlan sequenceDefinitionPlan)
        {
            var playerPlan = SequentioPooling.PopSequenceExecution();
            
            playerPlan.Duration = sequenceDefinitionPlan.Duration;
            playerPlan.Settings = sequenceDefinitionPlan.Settings.Clone();
            playerPlan.Storage = sequenceDefinitionPlan.Storage.Clone();
            
            foreach (var stepDefinitionPlan in sequenceDefinitionPlan.Steps)
            {
                var stepExecution = SequentioPooling.PopStepExecution();
                
                stepExecution.Definition = stepDefinitionPlan.Definition;
                stepExecution.Player = CreateStepPlayer(stepDefinitionPlan.Definition);
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
        
        internal static void RegisterStep<TStepDefinition, TStepPlayer>()
            where TStepDefinition : StepDefinition
            where TStepPlayer : StepPlayer<TStepDefinition>, new()
        {
            stepCompilers[typeof(TStepDefinition)] = new StepCompiler<TStepDefinition, TStepPlayer>();
        }
        
        private static StepPlayer CreateStepPlayer(StepDefinition stepDefinition)
        {
            return !stepCompilers.TryGetValue(stepDefinition.GetType(), out var stepCompiler) 
                ? throw new ArgumentException($"Invalid step definition type: {stepDefinition.GetType().FullName}") 
                : stepCompiler.Compile(stepDefinition);
        }
        
         private static void CreateStepsDefinition(SequencePlan plan, ISequenceDefinition definition, bool runtime)
         {
             for (var index = 0; index < definition.Steps.Count; index++)
             {
                 var stepDefinition = definition.Steps[index];
                 if (!stepDefinition.Visibility.IsEnabled(plan.Storage, runtime)) continue;

                 var stepPlan = SequentioPooling.PopStepDefinitionPlan();
                 stepPlan.Definition = stepDefinition;
                 stepPlan.RunWithPreviousStep = stepDefinition.RunWithPreviousStep;
                 stepPlan.InitOnStart = stepDefinition.InitOnStart;
                 stepPlan.Delay = stepDefinition.Delay.Evaluate(plan.Storage);
                 stepPlan.Duration = stepDefinition.CalculateDuration(plan.Storage);
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
