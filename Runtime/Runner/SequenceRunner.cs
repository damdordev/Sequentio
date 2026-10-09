using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    internal class SequenceRunner
    {
        public float Duration { get; private set; }
        public float CurrentTime { get; private set; }
        
        private List<SequenceStep> steps = new();
        private SequenceSettings settings;
        private readonly List<SequenceRunnerStepData> data = new();
        private VarioStorage storage;
        private bool runtime;
        private PlayDirection direction;
        
        public void Setup(bool runtime, VarioStorage storage, List<SequenceStep> steps, SequenceSettings settings)
        {
            this.runtime = runtime;
            this.storage = storage;
            this.steps = steps;
            this.settings = settings.Clone();
            direction = settings.Direction;
            
            ClearStepData();
            CreateStepData();
            RecalculateStepsTimes();
            InitSteps();
            CalculateDurationAndInitialTime();
        }

        internal void Reset()
        {
            Duration = 0f;
            CurrentTime = 0f;
            storage = null;
            steps = null;
            SequentioPooling.ReleaseSettings(settings);
            settings = null;
            ClearStepData();
        }
        
        public SequenceRunnerUpdateResult Update(float dt)
        {
            var factor = direction == PlayDirection.Normal ? 1f : -1f;
            var newTime = CurrentTime + dt * factor;

            if (newTime < 0f || newTime > Duration)
            {
                switch (settings.Loop)
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

            if (settings.Loop == LoopMode.None && direction == PlayDirection.Normal && newTime >= Duration)
            {
                return SequenceRunnerUpdateResult.Finished;
            }
            
            if (settings.Loop == LoopMode.None && direction == PlayDirection.Reverse && newTime <= 0f)
            {
                return SequenceRunnerUpdateResult.Finished;
            }

            return SequenceRunnerUpdateResult.NotFinished;
        }

        public void SetTime(float newTime)
        {
            CurrentTime = Mathf.Clamp(newTime, 0f, Duration);

            for (var index = 0; index < data.Count; index++)
            {
                var stepData = data[index];
                if (CurrentTime < stepData.StartTime) continue;

                if (!stepData.ValuesApplied)
                {
                    // TODO: error handling
                    // Debug.Log($"[Sequentio] Init step {index}");
                    stepData.Step.Init();
                    stepData.ValuesApplied = true;
                }

                var t = Mathf.InverseLerp(stepData.StartTime, stepData.EndTime, CurrentTime);
                if (CurrentTime >= stepData.EndTime) t = 1f;
                // Debug.Log($"[Sequentio] SetProgres({index}, {t})");
                stepData.Step.SetProgress(t);
            }
        }

        public void ReturnToInitialState()
        {
            for (var index = data.Count - 1; index >= 0; index--)
            {
                data[index].Step.ReturnToInitialState();
            }
            ClearStepData();

            CurrentTime = 0f;
        }
        
        private void ClearStepData()
        {
            foreach (var stepData in data)
            {
                SequentioPooling.ReleaseRunnerStepData(stepData);
            }
            data.Clear();
        }

        private void CreateStepData()
        {
            foreach (var step in steps)
            {
                step.Storage = storage;
                if (!step.IsEnabled(runtime)) continue;
                
                var stepData = SequentioPooling.PopRunnerStepData();
                stepData.Step = step;
                stepData.ValuesApplied = false;
                stepData.Append = step.RunWithPreviousStep;
                stepData.InitOnStart = step.InitOnStart;
                stepData.Delay = step.Delay.Evaluate(storage);
                stepData.Duration = step.CalculateDuration();
                
                data.Add(stepData);
            }
        }

        private void RecalculateStepsTimes()
        {
            var groupStartTime = 0f;
            var groupDuration = 0f;
            
            for (var i = 0; i < data.Count; i++)
            {
                if (data[i].Append)
                {
                    groupDuration = Mathf.Max(groupDuration, data[i].Duration);
                }
                else
                {
                    groupStartTime += groupDuration;
                    groupDuration = data[i].Duration + data[i].Delay;
                }

                data[i].StartTime = groupStartTime + data[i].Delay;
                data[i].EndTime = groupStartTime + data[i].Duration + data[i].Delay;
            }
        }

        private void InitSteps()
        {
            for (var index = 0; index < data.Count; index++)
            {
                var stepData = data[index];
                if (!stepData.InitOnStart) continue;
                // Debug.Log($"[Sequentio] Init step {index}");
                stepData.Step.Init();
                stepData.ValuesApplied = true;
                // Debug.Log($"[Sequentio] SetProgres({index}, 0)");
                stepData.Step.SetProgress(0f);
            }
        }
        
        private void CalculateDurationAndInitialTime()
        {
            Duration = 0f;
            foreach (var stepData in data) Duration = Mathf.Max(Duration, stepData.EndTime);
            CurrentTime = direction == PlayDirection.Normal ? 0f : Duration;
        }
        
    }
}