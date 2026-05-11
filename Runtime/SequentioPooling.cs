using System;
using System.Collections.Generic;

namespace Damdor.Sequentio
{
    internal static class SequentioPooling
    {
        
        public static int StepsPoolSize
        {
            get => stepsPoolSize;
            set
            {
                stepsPoolSize = value;
                foreach (var pool in stepsPools.Values) ApplyMaxPoolSize(pool, runnerPoolSize);
            }
        }
        
        public static int RunnerPoolSize
        {
            get => runnerPoolSize;
            set
            {
                runnerPoolSize = value;
                ApplyMaxPoolSize(runnerPool, runnerPoolSize);
            }
        }
        
        public static int RawSequencePoolSize
        {
            get => rawSequencePoolSize;
            set
            {
                rawSequencePoolSize = value;
                ApplyMaxPoolSize(rawSequencePool, rawSequencePoolSize);
            }
        }
        
        public static int SettingsPoolSize
        {
            get => settingsPoolSize;
            set
            {
                settingsPoolSize = value;
                ApplyMaxPoolSize(settingsPool, settingsPoolSize);
            }
        }
        
        public static int RunnerStepDataPoolSize
        {
            get => runnerStepDataPoolSize;
            set
            {
                runnerStepDataPoolSize = value;
                ApplyMaxPoolSize(runnerStepDataPool, runnerStepDataPoolSize);
            }
        }
        
        private static readonly Dictionary<Type, Stack<SequenceStep>> stepsPools = new();
        private static readonly Stack<SequenceRunner> runnerPool = new();
        private static readonly Stack<RawSequence> rawSequencePool = new();
        private static readonly Stack<SequenceSettings> settingsPool = new();
        private static readonly Stack<SequenceRunnerStepData> runnerStepDataPool = new();
        
        private static int stepsPoolSize = 100;
        private static int runnerPoolSize = 100;
        private static int rawSequencePoolSize = 100;
        private static int settingsPoolSize = 100;
        private static int runnerStepDataPoolSize = 100;
        
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
        
        private static void ApplyMaxPoolSize<T>(Stack<T> pool, int maxSize)
        {
            while (pool.Count > maxSize)
            {
                pool.Pop();
            }
        }
        
    }
}