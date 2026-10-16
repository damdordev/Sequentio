using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    internal class SequenceExecutionPlayer
    {
        public float Duration => data?.Duration ?? 0f;
        public float CurrentTime { get; private set; }
        
        private SequenceExecutionData data;
        private SequenceSettings settings;
        private PlayDirection direction;

        #region Pooling

        public static SequenceExecutionPlayer Create(SequenceExecutionData data, SequenceSettings settings)
        {
            var executionPlayer = SequentioPooling.PopSequenceExecutionPlayer();
            executionPlayer.Setup(data, settings);
            return executionPlayer;
        }
        
        private void Setup(SequenceExecutionData data, SequenceSettings settings)
        {
            this.settings = settings;
            this.data = data;
            direction = settings.Direction;
            
            InitSteps();
            CurrentTime = direction == PlayDirection.Normal ? 0f : Duration;
        }

        public void Release()
        {
            SequentioPooling.ReleaseSequenceExecutionPlayer(this);
        }
        
        internal void Reset()
        {
            CurrentTime = 0f;
            data = null;
            settings = null;
        }

        #endregion
        
        public SequenceExecutionUpdateResult Update(float dt)
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
                            if (newTime > Duration || newTime < 0f)
                            {
                                newTime = Mathf.PingPong(newTime, Duration);
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
                return SequenceExecutionUpdateResult.Finished;
            }
            
            if (settings.Loop == LoopMode.None && direction == PlayDirection.Reverse && newTime <= 0f)
            {
                return SequenceExecutionUpdateResult.Finished;
            }

            return SequenceExecutionUpdateResult.NotFinished;
        }

        public void SetTime(float newTime)
        {
            CurrentTime = Mathf.Clamp(newTime, 0f, Duration);

            // Rewind later steps first so they cannot overwrite earlier steps on the same target.
            for (var index = data.StepsData.Count - 1; index >= 0; index--)
            {
                var stepData = data.StepsData[index];
                if (CurrentTime >= stepData.StartTime || !stepData.ValuesApplied) continue;

                stepData.SetProgress(0f);
            }

            foreach (var stepData in data.StepsData)
            {
                if (CurrentTime < stepData.StartTime) continue;

                if (!stepData.ValuesApplied)
                {
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

        public void ReleaseSteps()
        {
            for (var index = data.StepsData.Count - 1; index >= 0; index--)
            {
                if (!data.StepsData[index].ValuesApplied) continue;
                data.StepsData[index].Step.Release();
            }
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
