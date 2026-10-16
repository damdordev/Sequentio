using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damdor.Sequentio
{
    internal static class SequentioPooling
    {
        
        public static int StepsPoolSize
        {
            get => stepsPoolSize;
            set
            {
                stepsPoolSize = Mathf.Max(value, 0);
                foreach (var pool in stepsPools.Values) ApplyMaxPoolSize(pool, runnerPoolSize);
            }
        }
        
        public static int RunnerPoolSize
        {
            get => runnerPoolSize;
            set
            {
                runnerPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(runnerPool, runnerPoolSize);
            }
        }
        
        public static int RawSequencePoolSize
        {
            get => rawSequencePoolSize;
            set
            {
                rawSequencePoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(rawSequencePool, rawSequencePoolSize);
            }
        }
        
        public static int SettingsPoolSize
        {
            get => settingsPoolSize;
            set
            {
                settingsPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(settingsPool, settingsPoolSize);
            }
        }
        
        public static int RunnerStepDataPoolSize
        {
            get => runnerStepDataPoolSize;
            set
            {
                runnerStepDataPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(runnerStepDataPool, runnerStepDataPoolSize);
            }
        }
        
        public static int RunnerDataPoolSize
        {
            get => runnerDataPoolSize;
            set
            {
                runnerDataPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(runnerDataPool, runnerDataPoolSize);
            }
        }
        
        public static int SequenceStateManagerPoolSize
        {
            get => sequenceStateManagerSize;
            set
            {
                sequenceStateManagerSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequenceStateManagerPool, sequenceStateManagerSize);
            }
        }
        
        private static readonly Dictionary<Type, Stack<SequenceStep>> stepsPools = new();
        private static readonly Stack<SequenceRunner> runnerPool = new();
        private static readonly Stack<RawSequence> rawSequencePool = new();
        private static readonly Stack<SequenceSettings> settingsPool = new();
        private static readonly Stack<SequenceRunnerStepData> runnerStepDataPool = new();
        private static readonly Stack<SequenceRunnerData> runnerDataPool = new();
        private static readonly Stack<SequenceStateManager> sequenceStateManagerPool = new();
        
        private static int stepsPoolSize = 100;
        private static int runnerPoolSize = 100;
        private static int rawSequencePoolSize = 100;
        private static int settingsPoolSize = 100;
        private static int runnerStepDataPoolSize = 100;
        private static int runnerDataPoolSize = 100;
        private static int sequenceStateManagerSize = 100;
        
        public static SequenceRunner PopRunner()
        {
            return runnerPool.Count > 0 
                ? runnerPool.Pop() 
                : new SequenceRunner();
        }

        public static void ReleaseRunner(SequenceRunner runner)
        {
            if (runner == null) return;
            runner.Reset();
            if(runnerPool.Count < runnerPoolSize) runnerPool.Push(runner);
        }
        
        public static RawSequence PopRawSequence()
        {
            return rawSequencePool.Count > 0 
                ? rawSequencePool.Pop() 
                : new RawSequence();
        }

        public static void ReleaseRawSequence(RawSequence sequence)
        {
            if (sequence == null) return;
            sequence.Reset();
            if(rawSequencePool.Count < rawSequencePoolSize) rawSequencePool.Push(sequence);
        }

        public static SequenceSettings PopSettings()
        {
            return settingsPool.Count > 0 
                ? settingsPool.Pop() 
                : new SequenceSettings();
        }

        public static void ReleaseSettings(SequenceSettings settings)
        {
            if (settings == null) return;
            settings.Reset();
            if(settingsPool.Count < settingsPoolSize) settingsPool.Push(settings);
        }
        
        public static T PopStep<T>() where T : SequenceStep, new()
        {
            var type = typeof(T);
            return !stepsPools.TryGetValue(type, out var pool) || pool.Count == 0
                ? new T()
                : (T)pool.Pop();
        }
        
        public static SequenceStep PopStep(Type type)
        {
            return !stepsPools.TryGetValue(type, out var pool) || pool.Count == 0
                ? (SequenceStep) Activator.CreateInstance(type)
                : pool.Pop();
        }
        
        public static void ReleaseStep(SequenceStep step)
        {
            if (step == null) return;
            var type = step.GetType();
            if (!stepsPools.TryGetValue(type, out var pool))
            {
                pool = new Stack<SequenceStep>();
                stepsPools[type] = pool;
            }
            step.Reset();
            if(pool.Count < stepsPoolSize) pool.Push(step);
        }
        
        public static SequenceRunnerStepData PopRunnerStepData()
        {
            return runnerStepDataPool.Count > 0 
                ? runnerStepDataPool.Pop() 
                : new SequenceRunnerStepData();
        }

        public static void ReleaseRunnerStepData(SequenceRunnerStepData stepData)
        {
            if (stepData == null) return;
            stepData.Reset();
            if(runnerStepDataPool.Count < runnerStepDataPoolSize) runnerStepDataPool.Push(stepData);
        }
        
        public static SequenceRunnerData PopRunnerData()
        {
            return runnerDataPool.Count > 0 
                ? runnerDataPool.Pop() 
                : new SequenceRunnerData();
        }

        public static void ReleaseRunnerData(SequenceRunnerData data)
        {
            if (data == null) return;
            data.Reset();
            if(runnerStepDataPool.Count < runnerStepDataPoolSize) runnerDataPool.Push(data);
        }
        
        public static SequenceStateManager PopSequenceStateManager()
        {
            return sequenceStateManagerPool.Count > 0 
                ? sequenceStateManagerPool.Pop() 
                : new SequenceStateManager();
        }

        public static void ReleaseSequenceStateManager(SequenceStateManager manager)
        {
            if (manager == null) return;
            manager.Reset();
            if(sequenceStateManagerPool.Count < sequenceStateManagerSize) sequenceStateManagerPool.Push(manager);
        }
        
        private static void ApplyMaxPoolSize<T>(Stack<T> pool, int maxSize)
        {
            while (pool.Count > maxSize)
            {
                pool.Pop();
            }
        }
        
    }
}