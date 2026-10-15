using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceRunnerData
    {
        public float Duration { get; private set; }
        public IReadOnlyList<SequenceRunnerStepData> StepsData => stepsData;
        
        private readonly List<SequenceRunnerStepData> stepsData = new();

        private IReadOnlyList<SequenceStep> steps;
        private VarioStorage storage;
        private bool runtime;
        
        public void Setup(IReadOnlyList<SequenceStep> steps, VarioStorage storage, bool runtime)
        {
            this.steps = steps;
            this.storage = storage;
            this.runtime = runtime;

            CreateStepData();
            RecalculateStepsTimes();
            CalculateDurationAndInitialTime();
        }

        public static SequenceRunnerData Create(IReadOnlyList<SequenceStep> steps, VarioStorage storage, bool runtime)
        {
            var data = SequentioPooling.PopRunnerData();
            data.Setup(steps, storage, runtime);
            return data;
        }

        public void Release()
        {
            SequentioPooling.ReleaseRunnerData(this);
        }

        internal void Reset()
        {
            steps = null;
            storage = null;
            runtime = false;
            
            foreach (var stepData in stepsData)
            {
                SequentioPooling.ReleaseRunnerStepData(stepData);
            }
            
            stepsData.Clear();
        }
        
        private void CreateStepData()
        {
            for (var index = 0; index < steps.Count; index++)
            {
                var step = steps[index];
                step.Storage = storage;
                if (!step.IsEnabled(runtime)) continue;

                var stepData = SequentioPooling.PopRunnerStepData();
                step.InvalidateDuration();
                stepData.Step = step;
                stepData.ValuesApplied = false;
                stepData.Append = step.RunWithPreviousStep;
                stepData.InitOnStart = step.InitOnStart;
                stepData.Delay = step.Delay.Evaluate(storage);
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
            
            for (var i = 0; i < stepsData.Count; i++)
            {
                if (stepsData[i].Append)
                {
                    groupDuration = Mathf.Max(groupDuration, stepsData[i].Duration + stepsData[i].Delay);
                }
                else
                {
                    groupSize = 0;
                    groupStartTime += groupDuration;
                    groupDuration = stepsData[i].Duration + stepsData[i].Delay;
                }

                stepsData[i].StartTime = groupStartTime + stepsData[i].Delay;
                stepsData[i].EndTime = groupStartTime + stepsData[i].Duration + stepsData[i].Delay;
                stepsData[i].IndexInGroup = groupSize++;
            }
        }
        
        private void CalculateDurationAndInitialTime()
        {
            Duration = 0f;
            foreach (var stepData in stepsData) Duration = Mathf.Max(Duration, stepData.EndTime);
        }
    }
}