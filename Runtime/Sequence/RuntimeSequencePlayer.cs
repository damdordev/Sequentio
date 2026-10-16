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
    
    public class RuntimeSequencePlayer : ISequencePlayer, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storage;
        public SequenceState State => playbackController?.State ?? SequenceState.Idle;
        public float Duration => playbackController?.Duration ?? 0f;
        public float CurrentTime => playbackController?.CurrentTime ?? 0f;

        private readonly List<SequenceStep> steps = new();
        private VarioStorage storage;
        private SequenceSettings settings;
        private bool autoRelease;
        private CancellationToken autoReleaseCancellationToken;

        private SequencePlaybackController playbackController;

        internal RuntimeSequencePlayer() { }

        #region Pooling

        public static RuntimeSequencePlayer Create(ISequenceDefinition sequenceDefinition, bool allowAutoplay = true)
        {
            var sequence = SequentioPooling.PopRuntimeSequencePlayer();
            sequence.Setup(sequenceDefinition);
            if(allowAutoplay) sequence.TryAutoPlay();
            return sequence;
        }
        
        private void Setup(ISequenceDefinition sequenceDefinition)
        {
            storage = sequenceDefinition.Storage.Clone();
            settings = sequenceDefinition.Settings.Clone();
            foreach (var step in sequenceDefinition.Steps) steps.Add(step.Clone());
            playbackController = SequencePlaybackController.Create(storage, settings, steps);
        }
        
        public void Release()
        {
            Stop(StopBehaviour.LeaveInCurrentState);
            SequentioPooling.ReleaseRuntimeSequencePlayer(this);
        }
        
        internal void Reset()
        {
            storage.Release();
            storage = null;
            playbackController?.Release();
            playbackController = null;
            autoReleaseCancellationToken = CancellationToken.None;
            if (steps != null)
            {
                foreach (var step in steps) SequentioPooling.ReleaseStep(step);
                steps.Clear();
            }

            if (settings != null)
            {
                SequentioPooling.ReleaseSequenceSettings(settings);
                settings = null;
            }
            
            autoRelease = false;
        }

        #endregion
        
    

        public void AutoRelease(GameObject go = null)
        {
            autoRelease = true;
            if (go != null) autoReleaseCancellationToken = go.GetCancellationTokenOnDestroy();
            if(playbackController.State == SequenceState.Finished || autoReleaseCancellationToken.IsCancellationRequested) Release();
        }

        public void AutoRelease(Component component = null)
        {
            var go = component != null ? component.gameObject : null;
            AutoRelease(go);
        }
        
        public void Init() => playbackController?.Init();

        public void Play()
        {
            if(playbackController.State == SequenceState.Idle)
            {
                
                SequentioUpdateProvider.Register(Update);
            };
            playbackController?.Play();
        }

        public void Pause() => playbackController?.Pause();
        public void Stop() => Stop(settings.DefaultStopBehaviour);

        public void Stop(StopBehaviour stopBehaviour)
        {
            playbackController?.Stop(stopBehaviour);
            SequentioUpdateProvider.Unregister(Update);
        }

        public void MoveToTime(float time) =>playbackController?.MoveToTime(time);
        public float GetProgress(SequenceStep step) => playbackController?.GetProgress(step) ?? 0f;
        public UniTask PlayAndWait() => playbackController?.PlayAndWait() ?? UniTask.CompletedTask;
        
        public void AddStep(SequenceStep step) => steps.Add(step);

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
        
        public void UpdateTime(float dt) => playbackController?.UpdateTime(dt);
        
    }
}