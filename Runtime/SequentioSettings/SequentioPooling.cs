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

        /// <summary>
        /// Gets or sets the maximum number of retained players per step player type.
        /// </summary>
        public static int StepPlayerPoolSize
        {
            get => stepPlayerPoolSize;
            set
            {
                stepPlayerPoolSize = Mathf.Max(value, 0);
                foreach (var pool in stepPlayerPools.Values) pool.MaxSize = stepPlayerPoolSize;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of retained sequence settings.
        /// </summary>
        public static int SequenceSettingsPoolSize
        {
            get => sequenceSettingsPool.MaxSize;
            set => sequenceSettingsPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained step plans.
        /// </summary>
        public static int StepDefinitionPlan
        {
            get => stepDefinitionPlanPool.MaxSize;
            set => stepDefinitionPlanPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained sequence plans.
        /// </summary>
        public static int SequenceDefinitionPlan
        {
            get => sequenceDefinitionPlanPool.MaxSize;
            set => sequenceDefinitionPlanPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained step executions.
        /// </summary>
        public static int StepPlayerPlan
        {
            get => stepPlayerPlanPool.MaxSize;
            set => stepPlayerPlanPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained sequence executions.
        /// </summary>
        public static int SequencePlayerPlan
        {
            get => sequencePlayerPlanPool.MaxSize;
            set => sequencePlayerPlanPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained sequence executors.
        /// </summary>
        public static int SequencePlayerPlanRunner
        {
            get => sequencePlayerPlanRunnerPool.MaxSize;
            set => sequencePlayerPlanRunnerPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained runtime sequences.
        /// </summary>
        public static int SequencePlaybackPlayerPoolSize
        {
            get => sequencePlaybackRunnerPool.MaxSize;
            set => sequencePlaybackRunnerPool.MaxSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of retained sequence definitions.
        /// </summary>
        public static int SequenceDefinitionPoolSize
        {
            get => sequenceDefinitionPool.MaxSize;
            set => sequenceDefinitionPool.MaxSize = value;
        }

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <typeparam name="T">The concrete step player type to obtain from the pool.</typeparam>
        /// <returns>The pooled or newly created instance.</returns>
        public static T PopStepPlayer<T>() where T : StepPlayer, new()
        {
            return (T)GetStepPlayerPool(typeof(T)).Pop(() => new T());
        }

        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="step">The instance to reset and return to its pool.</param>
        public static void ReleaseStepPlayer(StepPlayer step)
        {
            if (step == null) return;
            GetStepPlayerPool(step.GetType()).Release(step);
        }

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static SequenceSettings PopSequenceSettings() => sequenceSettingsPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="settings">The instance to reset and return to its pool.</param>
        public static void ReleaseSequenceSettings(SequenceSettings settings) => sequenceSettingsPool.Release(settings);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static StepPlan PopStepDefinitionPlan() => stepDefinitionPlanPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="plan">The instance to reset and return to its pool.</param>
        public static void ReleaseStepDefinitionPlan(StepPlan plan) => stepDefinitionPlanPool.Release(plan);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static SequencePlan PopSequenceDefinitionPlan() => sequenceDefinitionPlanPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="plan">The instance to reset and return to its pool.</param>
        public static void ReleaseSequenceDefinitionPlan(SequencePlan plan) => sequenceDefinitionPlanPool.Release(plan);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static StepExecution PopStepExecution() => stepPlayerPlanPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="plan">The instance to reset and return to its pool.</param>
        public static void ReleaseStepExecution(StepExecution plan) => stepPlayerPlanPool.Release(plan);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static SequenceExecution PopSequenceExecution() => sequencePlayerPlanPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="plan">The instance to reset and return to its pool.</param>
        public static void ReleaseSequenceExecution(SequenceExecution plan) => sequencePlayerPlanPool.Release(plan);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static SequenceExecutor PopSequenceExecutor() => sequencePlayerPlanRunnerPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="runner">The instance to reset and return to its pool.</param>
        public static void ReleaseSequenceExecutor(SequenceExecutor runner) => sequencePlayerPlanRunnerPool.Release(runner);

        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static RuntimeSequence PopSequenceRuntimeSequence() => sequencePlaybackRunnerPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="runner">The instance to reset and return to its pool.</param>
        public static void ReleaseRuntimeSequence(RuntimeSequence runner) => sequencePlaybackRunnerPool.Release(runner);
        
        /// <summary>
        /// Obtains an instance from the corresponding Sequentio pool, creating one if the pool is empty.
        /// </summary>
        /// <returns>The pooled or newly created instance.</returns>
        public static SequenceDefinition PopSequenceDefinition() => sequenceDefinitionPool.Pop();
        /// <summary>
        /// Resets the supplied instance and returns it to the corresponding Sequentio pool.
        /// </summary>
        /// <param name="definition">The definition used to configure playback.</param>
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
