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
                foreach (var pool in stepsPools.Values) ApplyMaxPoolSize(pool, sequenceExecutionPlayerPoolSize);
            }
        }
        
        public static int SequenceExecutionPlayerPoolSize
        {
            get => sequenceExecutionPlayerPoolSize;
            set
            {
                sequenceExecutionPlayerPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequenceExecutionPlayerPool, sequenceExecutionPlayerPoolSize);
            }
        }
        
        public static int SequenceStepExecutionDataPoolSize
        {
            get => sequenceStepExecutionDataPoolSize;
            set
            {
                sequenceStepExecutionDataPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequenceStepExecutionDataPool, sequenceStepExecutionDataPoolSize);
            }
        }
        
        public static int SequenceExecutionDataPoolSize
        {
            get => sequenceExecutionDataPoolSize;
            set
            {
                sequenceExecutionDataPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequenceExecutionDataPool, sequenceExecutionDataPoolSize);
            }
        }
        
        public static int RuntimeSequencePlayerPoolSize
        {
            get => runtimeSequencePlayerPoolSize;
            set
            {
                runtimeSequencePlayerPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(runtimeSequencePlayerPool, runtimeSequencePlayerPoolSize);
            }
        }
        
        public static int SequenceSettingsPoolSize
        {
            get => sequenceSettingsPoolSize;
            set
            {
                sequenceSettingsPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequenceSettingsPool, sequenceSettingsPoolSize);
            }
        }
        
        public static int SequencePlaybackControllerPoolSize
        {
            get => sequencePlaybackControllerPoolSize;
            set
            {
                sequencePlaybackControllerPoolSize = Mathf.Max(value, 0);
                ApplyMaxPoolSize(sequencePlaybackControllerPool, sequencePlaybackControllerPoolSize);
            }
        }
        
        private static readonly Dictionary<Type, Stack<SequenceStep>> stepsPools = new();
        private static readonly Stack<SequenceExecutionPlayer> sequenceExecutionPlayerPool = new();
        private static readonly Stack<RuntimeSequencePlayer> runtimeSequencePlayerPool = new();
        private static readonly Stack<SequenceSettings> sequenceSettingsPool = new();
        private static readonly Stack<SequenceStepExecutionData> sequenceStepExecutionDataPool = new();
        private static readonly Stack<SequenceExecutionData> sequenceExecutionDataPool = new();
        private static readonly Stack<SequencePlaybackController> sequencePlaybackControllerPool = new();
        
        private static int stepsPoolSize = 100;
        private static int sequenceExecutionPlayerPoolSize = 100;
        private static int runtimeSequencePlayerPoolSize = 100;
        private static int sequenceSettingsPoolSize = 100;
        private static int sequenceStepExecutionDataPoolSize = 100;
        private static int sequenceExecutionDataPoolSize = 100;
        private static int sequencePlaybackControllerPoolSize = 100;
        
        public static SequenceExecutionPlayer PopSequenceExecutionPlayer()
        {
            return sequenceExecutionPlayerPool.Count > 0 
                ? sequenceExecutionPlayerPool.Pop() 
                : new SequenceExecutionPlayer();
        }

        public static void ReleaseSequenceExecutionPlayer(SequenceExecutionPlayer runner)
        {
            if (runner == null) return;
            runner.Reset();
            if(sequenceExecutionPlayerPool.Count < sequenceExecutionPlayerPoolSize) sequenceExecutionPlayerPool.Push(runner);
        }
        
        public static RuntimeSequencePlayer PopRuntimeSequencePlayer()
        {
            return runtimeSequencePlayerPool.Count > 0 
                ? runtimeSequencePlayerPool.Pop() 
                : new RuntimeSequencePlayer();
        }

        public static void ReleaseRuntimeSequencePlayer(RuntimeSequencePlayer sequencePlayer)
        {
            if (sequencePlayer == null) return;
            sequencePlayer.Reset();
            if(runtimeSequencePlayerPool.Count < runtimeSequencePlayerPoolSize) runtimeSequencePlayerPool.Push(sequencePlayer);
        }

        public static SequenceSettings PopSequenceSettings()
        {
            return sequenceSettingsPool.Count > 0 
                ? sequenceSettingsPool.Pop() 
                : new SequenceSettings();
        }

        public static void ReleaseSequenceSettings(SequenceSettings settings)
        {
            if (settings == null) return;
            settings.Reset();
            if(sequenceSettingsPool.Count < sequenceSettingsPoolSize) sequenceSettingsPool.Push(settings);
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
        
        public static SequenceStepExecutionData PopSequenceStepExecutionData()
        {
            return sequenceStepExecutionDataPool.Count > 0 
                ? sequenceStepExecutionDataPool.Pop() 
                : new SequenceStepExecutionData();
        }

        public static void ReleaseSequenceStepExecutionData(SequenceStepExecutionData stepExecutionData)
        {
            if (stepExecutionData == null) return;
            stepExecutionData.Reset();
            if(sequenceStepExecutionDataPool.Count < sequenceStepExecutionDataPoolSize) sequenceStepExecutionDataPool.Push(stepExecutionData);
        }
        
        public static SequenceExecutionData PopSequenceExecutionData()
        {
            return sequenceExecutionDataPool.Count > 0 
                ? sequenceExecutionDataPool.Pop() 
                : new SequenceExecutionData();
        }

        public static void ReleaseSequenceExecutionData(SequenceExecutionData data)
        {
            if (data == null) return;
            data.Reset();
            if(sequenceStepExecutionDataPool.Count < sequenceStepExecutionDataPoolSize) sequenceExecutionDataPool.Push(data);
        }
        
        public static SequencePlaybackController PopSequencePlaybackController()
        {
            return sequencePlaybackControllerPool.Count > 0 
                ? sequencePlaybackControllerPool.Pop() 
                : new SequencePlaybackController();
        }

        public static void ReleaseSequencePlaybackController(SequencePlaybackController manager)
        {
            if (manager == null) return;
            manager.Reset();
            if(sequencePlaybackControllerPool.Count < sequencePlaybackControllerPoolSize) sequencePlaybackControllerPool.Push(manager);
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