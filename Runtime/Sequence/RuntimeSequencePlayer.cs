using System;
using System.Collections.Generic;
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
        private bool hasReleaseWatch;
        private GameObject releaseWatch;

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
            autoRelease = false;
            hasReleaseWatch = false;
            releaseWatch = null;
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

        public void AutoRelease()
        {
            autoRelease = true;
        }
        
        public void AutoRelease(GameObject go)
        {
            AutoRelease();
            if (go == null)
            {
                Release();
                return;
            }
            hasReleaseWatch = true;
            releaseWatch = go;
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
        
        #if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => playbackController?.PlayAndWait() ?? UniTask.CompletedTask;
        #endif
        
        public void AddStep(SequenceStep step) => steps.Add(step);

        private void TryAutoPlay()
        {
            if (Settings.AutoPlay == AutoPlayMode.None) return;
            Play();
        }

        private void Update(float deltaTime, float unscaledDeltaTime)
        {
            if (CheckAutoReleaseWatcher())
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

        private bool CheckAutoReleaseWatcher() => autoRelease && hasReleaseWatch && releaseWatch == null;
    }
}