using System;
using System.Collections.Generic;
using System.Threading;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    
    public class RawSequence : ISequence, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storage;
        public SequenceState State => stateManager?.State ?? SequenceState.Idle;
        public float Duration => stateManager?.Duration ?? 0f;
        public float CurrentTime => stateManager?.CurrentTime ?? 0f;

        private readonly List<SequenceStep> steps = new();
        private VarioStorage storage;
        private SequenceSettings settings;
        private bool autoRelease;
        private CancellationToken autoReleaseCancellationToken;

        private SequenceStateManager stateManager;

        internal RawSequence() { }
        
        public static RawSequence Create(ISequenceAsset sequenceAsset, bool allowAutoplay = true)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.Setup(sequenceAsset);
            if(allowAutoplay) sequence.TryAutoPlay();
            return sequence;
        }
        
        public void Release()
        {
            Stop(StopBehaviour.LeaveInCurrentState);
            SequentioPooling.ReleaseRawSequence(this);
        }
        
        internal void Reset()
        {
            storage.Release();
            storage = null;
            stateManager?.Release();
            stateManager = null;
            autoReleaseCancellationToken = CancellationToken.None;
            if (steps != null)
            {
                foreach (var step in steps) SequentioPooling.ReleaseStep(step);
                steps.Clear();
            }

            if (settings != null)
            {
                SequentioPooling.ReleaseSettings(settings);
                settings = null;
            }
            
            autoRelease = false;
        }

        public void AutoRelease(GameObject go = null)
        {
            autoRelease = true;
            if (go != null) autoReleaseCancellationToken = go.GetCancellationTokenOnDestroy();
            if(stateManager.State == SequenceState.Finished || autoReleaseCancellationToken.IsCancellationRequested) Release();
        }

        public void AutoRelease(Component component = null)
        {
            var go = component != null ? component.gameObject : null;
            AutoRelease(go);
        }
        
        public void Init() => stateManager?.Init();

        public void Play()
        {
            if(stateManager.State == SequenceState.Idle)
            {
                
                SequentioUpdateProvider.Register(Update);
            };
            stateManager?.Play();
        }

        public void Pause() => stateManager?.Pause();
        public void Stop() => Stop(settings.DefaultStopBehaviour);

        public void Stop(StopBehaviour stopBehaviour)
        {
            stateManager?.Stop(stopBehaviour);
            SequentioUpdateProvider.Unregister(Update);
        }

        public void MoveToTime(float time) =>stateManager?.MoveToTime(time);
        public float GetProgress(SequenceStep step) => stateManager?.GetProgress(step) ?? 0f;
        public UniTask PlayAndWait() => stateManager?.PlayAndWait() ?? UniTask.CompletedTask;
        
        public void AddStep(SequenceStep step) => steps.Add(step);
        
        private void Setup(ISequenceAsset sequenceAsset)
        {
            storage = sequenceAsset.Storage.Clone();
            settings = sequenceAsset.Settings.Clone();
            foreach (var step in sequenceAsset.Steps) steps.Add(step.Clone());
            stateManager = SequenceStateManager.Create(storage, settings, steps);
        }

        private void TryAutoPlay()
        {
            if (Settings.AutoPlay == AutoPlayMode.None) return;
            Play();
        }

        private void Update(float deltaTime, float unscaledDeltaTime)
        {
            if (autoRelease && autoReleaseCancellationToken.IsCancellationRequested)
            {
                Release();
                return;
            }
            var dt = settings.Timescale switch
            {
                TimescaleMode.Normal => deltaTime,
                TimescaleMode.Unscaled => unscaledDeltaTime,
                _ => throw new ArgumentOutOfRangeException()
            };
            UpdateTime(dt);
        }
        
        public void UpdateTime(float dt) => stateManager?.UpdateTime(dt);
        
    }
}