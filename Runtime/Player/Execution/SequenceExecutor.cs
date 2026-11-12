using System;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public class SequenceExecutor
    {
        private SequenceExecution execution;
        private PlayDirection direction;

        public static SequenceExecutor Create(SequenceExecution execution)
        {
            var player = SequentioPooling.PopSequenceExecutor();
            player.Setup(execution);
            return player;
        }

        private void Setup(SequenceExecution execution)
        {
            this.execution = execution;
            InitSteps();
            direction = execution.Settings.Direction;
            this.execution.CurrentTime = direction == PlayDirection.Forward ? 0f : this.execution.Duration;
        }

        public SequenceExecutorUpdateResult Update(float dt)
        {
            dt *= execution.Settings.Speed;
            var factor = direction == PlayDirection.Forward ? 1f : -1f;
            var newTime = execution.CurrentTime + dt * factor;

            if (newTime < 0f || newTime > execution.Duration)
            {
                switch (execution.Settings.Loop)
                {
                    case LoopMode.None:
                        newTime = Mathf.Clamp(newTime, 0f, execution.Duration);
                        break;
                    case LoopMode.Loop:
                        if (execution.Duration > 0f)
                        {
                            if (newTime > execution.Duration)
                            {
                                SetTime(execution.Duration);
                                newTime %= execution.Duration;
                            }
                            else if (newTime < 0f)
                            {
                                SetTime(0f);
                                newTime = execution.Duration + newTime % execution.Duration;
                            }
                        }
                        else
                        {
                            newTime = 0f;
                        }

                        break;
                    case LoopMode.PingPong:
                        if (execution.Duration > 0f)
                        {
                            if (newTime > execution.Duration)
                            {
                                if (Mathf.FloorToInt( newTime / execution.Duration) % 2 == 1)
                                {
                                    direction = direction == PlayDirection.Forward
                                        ? PlayDirection.Reverse
                                        : PlayDirection.Forward;
                                }                                
                                SetTime(execution.Duration);
                                newTime = Mathf.PingPong(newTime, execution.Duration);
                            }
                            else if (newTime < 0f)
                            {
                                if (Mathf.FloorToInt( -newTime / execution.Duration) % 2 == 0)
                                {
                                    direction = direction == PlayDirection.Forward
                                        ? PlayDirection.Reverse
                                        : PlayDirection.Forward;
                                }
                                SetTime(0f);
                                newTime = Mathf.PingPong(newTime, execution.Duration);
                            }
                        }
                        else
                        {
                            newTime = 0f;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            SetTime(newTime);

            if (execution.Settings.Loop == LoopMode.None && direction == PlayDirection.Forward && newTime >= execution.Duration)
            {
                return SequenceExecutorUpdateResult.Finished;
            }
            
            if (execution.Settings.Loop == LoopMode.None && direction == PlayDirection.Reverse && newTime <= 0f)
            {
                return SequenceExecutorUpdateResult.Finished;
            }

            return SequenceExecutorUpdateResult.NotFinished;
        }

        public void SetTime(float newTime)
        {
            execution.CurrentTime = Mathf.Clamp(newTime, 0f, execution.Duration);

            for (var index = execution.Steps.Count - 1; index >= 0; index--)
            {
                var stepData = execution.Steps[index];
                if (execution.CurrentTime >= stepData.StartTime || !stepData.ValuesApplied) continue;

                stepData.SetProgress(0f);
            }

            foreach (var stepData in execution.Steps)
            {
                if (execution.CurrentTime < stepData.StartTime) continue;

                if (!stepData.ValuesApplied)
                {
                    stepData.Init(execution.Storage);
                    stepData.ValuesApplied = true;
                }

                var t = Mathf.InverseLerp(stepData.StartTime, stepData.EndTime, execution.CurrentTime);
                if (execution.CurrentTime >= stepData.EndTime) t = 1f;
                stepData.SetProgress(t);
            }
        }

        public void ReturnToInitialState()
        {
            for (var index = execution.Steps.Count - 1; index >= 0; index--)
            {
                if (!execution.Steps[index].ValuesApplied) continue;
                execution.Steps[index].ReturnToInitialState();
            }

            execution.CurrentTime = 0f;
        }

        public void ReleaseSteps()
        {
            for (var index = execution.Steps.Count - 1; index >= 0; index--)
            {
                if (!execution.Steps[index].ValuesApplied) continue;
            }
        }

        public void Release()
        {
            SequentioPooling.ReleaseSequenceExecutor(this);
        }

        internal void Reset()
        {
            execution = null;
            direction = PlayDirection.Forward;
        }

        private void InitSteps()
        {
            foreach (var stepPlan in execution.Steps)
            {
                if (!stepPlan.InitOnStart) continue;
                stepPlan.Init(execution.Storage);
                stepPlan.ValuesApplied = true;
                stepPlan.SetProgress(0f);
            }
        }
    }
}