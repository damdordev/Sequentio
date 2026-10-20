using System;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public class SequenceExecutor
    {
        private SequenceExecution plan;
        private PlayDirection direction;

        public static SequenceExecutor Create(SequenceExecution plan)
        {
            var player = SequentioPooling.PopSequenceExecutor();
            player.Setup(plan);
            return player;
        }

        private void Setup(SequenceExecution plan)
        {
            this.plan = plan;
            InitSteps();
            direction = plan.Settings.Direction;
            this.plan.CurrentTime = direction == PlayDirection.Forward ? 0f : this.plan.Duration;
        }

        public SequenceExecutorUpdateResult Update(float dt)
        {
            dt *= plan.Settings.Speed;
            var factor = direction == PlayDirection.Forward ? 1f : -1f;
            var newTime = plan.CurrentTime + dt * factor;

            if (newTime < 0f || newTime > plan.Duration)
            {
                switch (plan.Settings.Loop)
                {
                    case LoopMode.None:
                        newTime = Mathf.Clamp(newTime, 0f, plan.Duration);
                        break;
                    case LoopMode.Loop:
                        if (plan.Duration > 0f)
                        {
                            if (newTime > plan.Duration) newTime %= plan.Duration;
                            else if (newTime < 0f) newTime = plan.Duration + newTime % plan.Duration;
                        }
                        else
                        {
                            newTime = 0f;
                        }

                        break;
                    case LoopMode.PingPong:
                        if (plan.Duration > 0f)
                        {
                            direction = direction == PlayDirection.Forward
                                ? PlayDirection.Reverse
                                : PlayDirection.Forward;
                            if (newTime > plan.Duration || newTime < 0f)
                            {
                                newTime = Mathf.PingPong(newTime, plan.Duration);
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

            if (plan.Settings.Loop == LoopMode.None && direction == PlayDirection.Forward &&
                newTime >= plan.Duration || plan.Settings.Loop == LoopMode.None && direction == PlayDirection.Reverse &&
                newTime <= 0f)
            {
                return SequenceExecutorUpdateResult.Finished;
            }

            return SequenceExecutorUpdateResult.NotFinished;
        }

        public void SetTime(float newTime)
        {
            plan.CurrentTime = Mathf.Clamp(newTime, 0f, plan.Duration);

            for (var index = plan.Steps.Count - 1; index >= 0; index--)
            {
                var stepData = plan.Steps[index];
                if (plan.CurrentTime >= stepData.StartTime || !stepData.ValuesApplied) continue;

                stepData.SetProgress(0f);
            }

            foreach (var stepData in plan.Steps)
            {
                if (plan.CurrentTime < stepData.StartTime) continue;

                if (!stepData.ValuesApplied)
                {
                    stepData.Player.Init(plan.Storage);
                    stepData.ValuesApplied = true;
                }

                var t = Mathf.InverseLerp(stepData.StartTime, stepData.EndTime, plan.CurrentTime);
                if (plan.CurrentTime >= stepData.EndTime) t = 1f;
                stepData.SetProgress(t);
            }
        }

        public void ReturnToInitialState()
        {
            for (var index = plan.Steps.Count - 1; index >= 0; index--)
            {
                if (!plan.Steps[index].ValuesApplied) continue;
                plan.Steps[index].Player.ReturnToInitialState();
            }

            plan.CurrentTime = 0f;
        }

        public void ReleaseSteps()
        {
            for (var index = plan.Steps.Count - 1; index >= 0; index--)
            {
                if (!plan.Steps[index].ValuesApplied) continue;
                plan.Steps[index].Player.Release();
            }
        }

        public void Release()
        {
            SequentioPooling.ReleaseSequenceExecutor(this);
        }

        internal void Reset()
        {
            plan = null;
            direction = PlayDirection.Forward;
        }

        private void InitSteps()
        {
            foreach (var stepPlan in plan.Steps)
            {
                if (!stepPlan.InitOnStart) continue;
                stepPlan.Player.Init(plan.Storage);
                stepPlan.ValuesApplied = true;
                stepPlan.SetProgress(0f);
            }
        }
    }
}