using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class SequentioRunner
    {
        private class StepData
        {
            public SequentioStep Step;
            public bool ValuesApplied;
            public float StartTime;
            public float EndTime;
        }
        
        private readonly List<SequentioStep> allSteps = new();
        private readonly List<StepData> steps = new();
        private readonly VarioStorage storage;
        private readonly bool runtime;
        private bool isInit;
        
        public SequentioRunner(bool runtime, VarioStorage storage, List<SequentioStep> allSteps)
        {
            this.runtime = runtime;
            this.storage = storage;
            this.allSteps = allSteps;
        }

        public void Init()
        {
            if (isInit) return;
            
            foreach (var step in allSteps) step.Storage = storage;
            steps.Clear();
            foreach (var step in allSteps)
            {
                if (step.IsEnabled(runtime)) steps.Add(new StepData
                {
                    Step = step,
                    ValuesApplied = false
                });
            }

            var time = 0f;
            foreach (var data in steps)
            {
                var duration = data.Step.CalculateDuration();
                data.StartTime = time;
                data.EndTime = data.StartTime + duration;
                time += duration;
            }
            foreach (var data in steps) data.Step.SaveInitialState();
            foreach (var data in steps)
            {
                if (data.Step.InitOnStart)
                {
                    data.Step.ApplyValues();
                    data.ValuesApplied = true;
                }
            }
        }
        
        public void Update(float dt)
        {
            
        }

        public void ReturnToInitialState()
        {
            for (var index = steps.Count - 1; index >= 0; index--)
            {
                steps[index].Step.ReturnToInitialState();
            }
        }
        
    }
}