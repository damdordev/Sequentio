using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceRunnerData
    {
        public float Duration { get; private set; }
        public List<SequenceRunnerStepData> StepsData => stepsData;
        
        private readonly List<SequenceRunnerStepData> stepsData = new();

        private List<SequenceStep> steps;
        private VarioStorage storage;
        private bool runtime;
        
        public void Setup(List<SequenceStep> steps, VarioStorage storage, bool runtime)
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
            foreach (var step in steps)
            {
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
                
                stepsData.Add(stepData);
            }
        }
        
        private void RecalculateStepsTimes()
        {
            var groupStartTime = 0f;
            var groupDuration = 0f;
            
            for (var i = 0; i < stepsData.Count; i++)
            {
                if (stepsData[i].Append)
                {
                    groupDuration = Mathf.Max(groupDuration, stepsData[i].Duration + stepsData[i].Delay);
                }
                else
                {
                    groupStartTime += groupDuration;
                    groupDuration = stepsData[i].Duration + stepsData[i].Delay;
                }

                stepsData[i].StartTime = groupStartTime + stepsData[i].Delay;
                stepsData[i].EndTime = groupStartTime + stepsData[i].Duration + stepsData[i].Delay;
            }
        }
        
        private void CalculateDurationAndInitialTime()
        {
            Duration = 0f;
            foreach (var stepData in stepsData) Duration = Mathf.Max(Duration, stepData.EndTime);
        }
    }
}