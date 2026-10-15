using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    internal class SequenceRunner
    {
        public float Duration => data?.Duration ?? 0f;
        public float CurrentTime { get; private set; }
        
        private SequenceSettings settings;
        private SequenceRunnerData data;
        private PlayDirection direction;
        
        public void Setup(bool runtime, VarioStorage storage, List<SequenceStep> steps, SequenceSettings settings)
        {
            this.settings = settings.Clone();
            direction = settings.Direction;

            data = SequenceRunnerData.Create(steps, storage, runtime);
            
            InitSteps();
            CurrentTime = direction == PlayDirection.Normal ? 0f : Duration;
        }

        internal void Reset()
        {
            CurrentTime = 0f;
            SequentioPooling.ReleaseSettings(settings);
            settings = null;
            data?.Release();
            data = null;
        }
        
        public SequenceRunnerUpdateResult Update(float dt)
        {
            dt *= settings.Speed;
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

            for (var index = 0; index < data.StepsData.Count; index++)
            {
                var stepData = data.StepsData[index];
                if (CurrentTime < stepData.StartTime) continue;

                if (!stepData.ValuesApplied)
                {
                    // TODO: error handling
                    stepData.Step.Init();
                    stepData.ValuesApplied = true;
                }

                var t = Mathf.InverseLerp(stepData.StartTime, stepData.EndTime, CurrentTime);
                if (CurrentTime >= stepData.EndTime) t = 1f;
                stepData.SetProgress(t);
            }
        }

        public void ReturnToInitialState()
        {
            for (var index = data.StepsData.Count - 1; index >= 0; index--)
            {
                if (!data.StepsData[index].ValuesApplied) continue;
                data.StepsData[index].Step.ReturnToInitialState();
            }

            CurrentTime = 0f;
        }

        public void Release()
        {
            for (var index = data.StepsData.Count - 1; index >= 0; index--)
            {
                if (!data.StepsData[index].ValuesApplied) continue;
                data.StepsData[index].Step.Release();
            }

            data?.Release();
            data = null;
        }

        public float GetProgress(SequenceStep step)
        {
            foreach (var stepData in data.StepsData)
            {
                if(stepData.Step == step) return stepData.Progress;
            }

            return 0f;
        }

        private void InitSteps()
        {
            foreach (var stepData in data.StepsData)
            {
                if (!stepData.InitOnStart) continue;
                stepData.Step.Init();
                stepData.ValuesApplied = true;
                stepData.SetProgress(0f);
            }
        }
        
    }
}