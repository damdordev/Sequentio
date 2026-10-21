// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damdor.Sequentio
{
    public static class SequenceCompiler
    {
        private static readonly Dictionary<Type, StepCompiler> stepCompilers = new();
        
        public static SequencePlan CreateDefinitionPlan(ISequenceDefinition definition, bool runtime = true)
        {
            var plan = SequentioPooling.PopSequenceDefinitionPlan();
            plan.Settings = definition.Settings.Clone();
            plan.Storage = definition.Storage.Clone();
           
            CreateStepsDefinition(plan, definition, runtime);
            RecalculateStepsTimes(plan);
            CalculateDurationAndInitialTime(plan);
            
            return plan;
        }
        
        public static SequenceExecution CreatePlayerPlan(SequencePlan sequenceDefinitionPlan)
        {
            var playerPlan = SequentioPooling.PopSequenceExecution();
            
            playerPlan.Duration = sequenceDefinitionPlan.Duration;
            playerPlan.Settings = sequenceDefinitionPlan.Settings;
            playerPlan.Storage = sequenceDefinitionPlan.Storage;
            
            foreach (var stepDefinitionPlan in sequenceDefinitionPlan.Steps)
            {
                var stepPlayerPlan = SequentioPooling.PopStepExecution();
                
                stepPlayerPlan.Definition = stepDefinitionPlan.Definition;
                stepPlayerPlan.Player = CreateStepPlayer(stepDefinitionPlan.Definition);
                stepPlayerPlan.StartTime = stepDefinitionPlan.StartTime;
                stepPlayerPlan.EndTime = stepDefinitionPlan.EndTime;
                stepPlayerPlan.Duration = stepDefinitionPlan.Duration;
                stepPlayerPlan.Delay = stepDefinitionPlan.Delay;
                stepPlayerPlan.InitOnStart = stepDefinitionPlan.InitOnStart;
                stepPlayerPlan.RunWithPreviousStep = stepDefinitionPlan.RunWithPreviousStep;

                playerPlan.Steps.Add(stepPlayerPlan);
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
                 if (!stepDefinition.Visibility.IsEnabled(definition.Storage, runtime)) continue;

                 var stepPlan = SequentioPooling.PopStepDefinitionPlan();
                 stepPlan.Definition = stepDefinition;
                 stepPlan.RunWithPreviousStep = stepDefinition.RunWithPreviousStep;
                 stepPlan.InitOnStart = stepDefinition.InitOnStart;
                 stepPlan.Delay = stepDefinition.Delay.Evaluate(definition.Storage);
                 stepPlan.Duration = stepDefinition.CalculateDuration(definition.Storage);
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