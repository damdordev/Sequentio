// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Damdor.Sequentio
{
    internal static class SequentioPooling
    {
        private static readonly Dictionary<Type, ObjectPool<StepPlayer>> stepPlayerPools = new();
        private static int stepPlayerPoolSize = 100;

        private static readonly ObjectPool<SequenceSettings> sequenceSettingsPool =
            new(100, settings => settings.Reset());
        private static readonly ObjectPool<StepPlan> stepDefinitionPlanPool =
            new(200, plan => plan.Reset(), true);
        private static readonly ObjectPool<SequencePlan> sequenceDefinitionPlanPool =
            new(100, plan => plan.Reset(), true);
        private static readonly ObjectPool<StepExecution> stepPlayerPlanPool =
            new(200, plan => plan.Reset(), true);
        private static readonly ObjectPool<SequenceExecution> sequencePlayerPlanPool =
            new(100, plan => plan.Reset(), true);
        private static readonly ObjectPool<SequenceExecutor> sequencePlayerPlanRunnerPool =
            new(200, runner => runner.Reset(), true);
        private static readonly ObjectPool<RuntimeSequence> sequencePlaybackRunnerPool =
            new(200, runner => runner.Reset(), true);
        private static readonly ObjectPool<SequenceDefinition> sequenceDefinitionPool =
            new(200, runner => runner.Reset(), true);

        public static int StepPlayerPoolSize
        {
            get => stepPlayerPoolSize;
            set
            {
                stepPlayerPoolSize = Mathf.Max(value, 0);
                foreach (var pool in stepPlayerPools.Values) pool.MaxSize = stepPlayerPoolSize;
            }
        }

        public static int SequenceSettingsPoolSize
        {
            get => sequenceSettingsPool.MaxSize;
            set => sequenceSettingsPool.MaxSize = value;
        }

        public static int StepDefinitionPlan
        {
            get => stepDefinitionPlanPool.MaxSize;
            set => stepDefinitionPlanPool.MaxSize = value;
        }

        public static int SequenceDefinitionPlan
        {
            get => sequenceDefinitionPlanPool.MaxSize;
            set => sequenceDefinitionPlanPool.MaxSize = value;
        }

        public static int StepPlayerPlan
        {
            get => stepPlayerPlanPool.MaxSize;
            set => stepPlayerPlanPool.MaxSize = value;
        }

        public static int SequencePlayerPlan
        {
            get => sequencePlayerPlanPool.MaxSize;
            set => sequencePlayerPlanPool.MaxSize = value;
        }

        public static int SequencePlayerPlanRunner
        {
            get => sequencePlayerPlanRunnerPool.MaxSize;
            set => sequencePlayerPlanRunnerPool.MaxSize = value;
        }

        public static int SequencePlaybackPlayerPoolSize
        {
            get => sequencePlaybackRunnerPool.MaxSize;
            set => sequencePlaybackRunnerPool.MaxSize = value;
        }

        public static int SequenceDefinitionPoolSize
        {
            get => sequenceDefinitionPool.MaxSize;
            set => sequenceDefinitionPool.MaxSize = value;
        }

        public static T PopStepPlayer<T>() where T : StepPlayer, new()
        {
            return (T)GetStepPlayerPool(typeof(T)).Pop(() => new T());
        }

        public static void ReleaseStepPlayer(StepPlayer step)
        {
            if (step == null) return;
            GetStepPlayerPool(step.GetType()).Release(step);
        }

        public static SequenceSettings PopSequenceSettings() => sequenceSettingsPool.Pop();
        public static void ReleaseSequenceSettings(SequenceSettings settings) => sequenceSettingsPool.Release(settings);

        public static StepPlan PopStepDefinitionPlan() => stepDefinitionPlanPool.Pop();
        public static void ReleaseStepDefinitionPlan(StepPlan plan) => stepDefinitionPlanPool.Release(plan);

        public static SequencePlan PopSequenceDefinitionPlan() => sequenceDefinitionPlanPool.Pop();
        public static void ReleaseSequenceDefinitionPlan(SequencePlan plan) => sequenceDefinitionPlanPool.Release(plan);

        public static StepExecution PopStepPlayerPlan() => stepPlayerPlanPool.Pop();
        public static void ReleaseStepPlayerPlan(StepExecution plan) => stepPlayerPlanPool.Release(plan);

        public static SequenceExecution PopSequencePlayerPlan() => sequencePlayerPlanPool.Pop();
        public static void ReleaseSequencePlayerPlan(SequenceExecution plan) => sequencePlayerPlanPool.Release(plan);

        public static SequenceExecutor PopSequencePlanPlayer() => sequencePlayerPlanRunnerPool.Pop();
        public static void ReleaseSequencePlanPlayer(SequenceExecutor runner) => sequencePlayerPlanRunnerPool.Release(runner);

        public static RuntimeSequence PopSequencePlaybackRunner() => sequencePlaybackRunnerPool.Pop();
        public static void ReleaseSequencePlaybackRunner(RuntimeSequence runner) => sequencePlaybackRunnerPool.Release(runner);
        
        public static SequenceDefinition PopSequenceDefinition() => sequenceDefinitionPool.Pop();
        public static void ReleaseSequenceDefinition(SequenceDefinition definition) => sequenceDefinitionPool.Release(definition);
        
        private static ObjectPool<StepPlayer> GetStepPlayerPool(Type type)
        {
            if (stepPlayerPools.TryGetValue(type, out var pool)) return pool;
            pool = new ObjectPool<StepPlayer>(stepPlayerPoolSize, step => step.Reset());
            stepPlayerPools[type] = pool;
            return pool;
        }

        private sealed class ObjectPool<T> where T : class, new()
        {
            private readonly Stack<T> items = new();
            private readonly Action<T> reset;
            private readonly bool resetOnCreate;
            private int maxSize;

            public ObjectPool(int maxSize, Action<T> reset, bool resetOnCreate = false)
            {
                this.reset = reset;
                this.resetOnCreate = resetOnCreate;
                MaxSize = maxSize;
            }

            public int MaxSize
            {
                get => maxSize;
                set
                {
                    maxSize = Mathf.Max(value, 0);
                    while (items.Count > maxSize) items.Pop();
                }
            }

            public T Pop()
            {
                if (items.Count > 0) return items.Pop();
                var item = new T();
                if (resetOnCreate) reset(item);
                return item;
            }
            
            public T Pop(Func<T> create)
            {
                if (items.Count > 0) return items.Pop();
                var item = create();
                if (resetOnCreate) reset(item);
                return item;
            }

            public void Release(T item)
            {
                if (item == null) return;
                reset(item);
                if (items.Count < maxSize) items.Push(item);
            }
        }
    }
}
