using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceRunner
    {
        private class StepData
        {
            public SequenceStep Step;
            public bool ValuesApplied;
            public float StartTime;
            public float EndTime;
            public float Duration;
            public bool Append;
            public bool InitOnStart;
        }
        
        public float Duration { get; private set; }
        public float CurrentTime { get; private set; }
        
        private readonly List<SequenceStep> steps = new();
        private readonly List<StepData> data = new();
        private readonly VarioStorage storage;
        private readonly bool runtime;
        private bool isInit;
        private LoopMode loopMode;
        private PlayDirection direction;
        
        public SequenceRunner(bool runtime, VarioStorage storage, List<SequenceStep> steps, LoopMode loopMode, PlayDirection direction)
        {
            this.runtime = runtime;
            this.storage = storage;
            this.steps = steps;
            this.loopMode = loopMode;
            this.direction = direction;
        }

        public void Init()
        {
            if (isInit) return;
            
            foreach (var step in steps) step.Storage = storage;
            data.Clear();
            foreach (var step in steps)
            {
                if (!step.IsEnabled(runtime)) continue;
                data.Add(CreateStepData(step));
            }

            foreach (var step in data) step.Duration = step.Step.CalculateDuration();

            if (data.Count > 0)
            {
                data[0].StartTime = 0f;
                data[0].EndTime = data[0].Duration;
            }

            for (var i = 1; i < data.Count; i++)
            {
                data[i].StartTime = data[i].Append ? data[i - 1].StartTime : data[i - 1].EndTime;
                data[i].EndTime = data[i].StartTime + data[i].Duration;
            }

            Duration = data.Count > 0 ? data[^1].EndTime : 0f;
            CurrentTime = direction == PlayDirection.Normal ? 0f : Duration;
            
            foreach (var data in data)
            {
                if (data.Step.InitOnStart)
                {
                    data.Step.Init();
                    data.ValuesApplied = true;
                    data.Step.SetProgress(0f);
                }
            }

            isInit = true;
        }
        
        public SequenceRunnerUpdateResult Update(float dt)
        {
            var factor = direction == PlayDirection.Normal ? 1f : -1f;
            var newTime = CurrentTime + dt * factor;

            if (newTime < 0f || newTime > Duration)
            {
                switch (loopMode)
                {
                    case LoopMode.None:
                        newTime = Mathf.Clamp(newTime, 0f, Duration);
                        break;
                    case LoopMode.Loop:
                        if (Duration > 0f)
                        {
                            if (newTime > Duration)
                            {
                                newTime %= Duration;
                            }
                            else if (newTime < 0f)
                            {
                                newTime = Duration + (newTime % Duration);
                            }
                        }
                        else
                        {
                            newTime = 0f;
                        }
                        break;
                    case LoopMode.PingPong:
                        if (Duration > 0f)
                        {
                            direction = direction == PlayDirection.Normal ? PlayDirection.Reverse : PlayDirection.Normal;
                            if (newTime > Duration)
                            {
                                newTime = Duration - (newTime - Duration);
                            }
                            else if (newTime < 0f)
                            {
                                newTime = -newTime;
                            }
                        }
                        else
                        {
                            newTime = 0f;
                        }
                        break;
                }
            }
            SetTime(newTime);

            if (loopMode == LoopMode.None && direction == PlayDirection.Normal && newTime >= Duration)
            {
                return SequenceRunnerUpdateResult.Finished;
            }
            
            if (loopMode == LoopMode.None && direction == PlayDirection.Reverse && newTime <= 0f)
            {
                return SequenceRunnerUpdateResult.Finished;
            }

            return SequenceRunnerUpdateResult.NotFinished;
        }

        public void SetTime(float newTime)
        {
            CurrentTime = Mathf.Clamp(newTime, 0f, Duration);
            
            foreach (var stepData in data)
            {
                if(CurrentTime < stepData.StartTime) continue;
                
                if (!stepData.ValuesApplied)
                {
                    // TODO: error handling
                    stepData.Step.Init();
                    stepData.ValuesApplied = true;
                }
                
                var t = Mathf.InverseLerp(stepData.StartTime, stepData.EndTime, CurrentTime);
                stepData.Step.SetProgress(t);
            }
        }

        public void ReturnToInitialState()
        {
            for (var index = data.Count - 1; index >= 0; index--)
            {
                data[index].Step.ReturnToInitialState();
            }
        }
        
        private StepData CreateStepData(SequenceStep step) => new()
        {
            Step = step,
            ValuesApplied = false,
            Append = false,
            InitOnStart = step.InitOnStart
        };
        
    }
}