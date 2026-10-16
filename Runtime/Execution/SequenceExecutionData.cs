using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceExecutionData
    {
        public float Duration { get; private set; }
        public IReadOnlyList<SequenceStepExecutionData> StepsData => stepsData;
        
        private readonly List<SequenceStepExecutionData> stepsData = new();

        private IReadOnlyList<SequenceStep> steps;
        private VarioStorage storage;
        private bool runtime;

        #region Pooling
        
        public static SequenceExecutionData Create(IReadOnlyList<SequenceStep> steps, VarioStorage storage, bool runtime)
        {
            var data = SequentioPooling.PopSequenceExecutionData();
            data.Setup(steps, storage, runtime);
            return data;
        }
        
        private void Setup(IReadOnlyList<SequenceStep> steps, VarioStorage storage, bool runtime)
        {
            this.steps = steps;
            this.storage = storage;
            this.runtime = runtime;
            CreateStepData();
            RecalculateStepsTimes();
            CalculateDurationAndInitialTime();
        }
        
        public void Release()
        {
            SequentioPooling.ReleaseSequenceExecutionData(this);
        }

        internal void Reset()
        {
            steps = null;
            storage = null;
            runtime = false;
            
            foreach (var stepData in stepsData)
            {
                SequentioPooling.ReleaseSequenceStepExecutionData(stepData);
            }
            
            stepsData.Clear();
        }
        
        #endregion
        
        private void CreateStepData()
        {
            for (var index = 0; index < steps.Count; index++)
            {
                var step = steps[index];
                step.Storage = storage;
                if (!step.IsEnabled(runtime)) continue;

                var stepData = SequentioPooling.PopSequenceStepExecutionData();
                step.InvalidateDuration();
                stepData.Step = step;
                stepData.ValuesApplied = false;
                stepData.Append = step.RunWithPreviousStep;
                stepData.InitOnStart = step.InitOnStart;
                stepData.Delay = step.CalculatedDelay;
                stepData.Duration = step.CalculatedDuration;
                stepData.OriginalIndex = index;

                stepsData.Add(stepData);
            }
        }
        
        private void RecalculateStepsTimes()
        {
            var groupStartTime = 0f;
            var groupDuration = 0f; 
            var groupSize = 0;
            
            foreach (var stepExecutionData in stepsData)
            {
                if (stepExecutionData.Append)
                {
                    groupDuration = Mathf.Max(groupDuration, stepExecutionData.Duration + stepExecutionData.Delay);
                }
                else
                {
                    groupSize = 0;
                    groupStartTime += groupDuration;
                    groupDuration = stepExecutionData.Duration + stepExecutionData.Delay;
                }

                stepExecutionData.StartTime = groupStartTime + stepExecutionData.Delay;
                stepExecutionData.EndTime = groupStartTime + stepExecutionData.Duration + stepExecutionData.Delay;
                stepExecutionData.IndexInGroup = groupSize++;
            }
        }
        
        private void CalculateDurationAndInitialTime()
        {
            Duration = 0f;
            foreach (var stepData in stepsData) Duration = Mathf.Max(Duration, stepData.EndTime);
        }
    }
}