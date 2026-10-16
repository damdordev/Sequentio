using System;
using System.Collections.Generic;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    
    public class SequencePlayer : MonoBehaviour, ISequencePlayer, ISequenceDefinition, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storage;
        public IReadOnlyList<SequenceStep> Steps => steps;
        
        public SequenceState State => PlaybackController?.State ?? SequenceState.Idle;
        public float Duration => PlaybackController?.Duration ?? 0f;
        public float CurrentTime => PlaybackController?.CurrentTime ?? 0f;

        public void Init() => PlaybackController?.Init();
        public void Play() => PlaybackController?.Play();
        public void Pause() => PlaybackController?.Pause();
        public void Stop() => PlaybackController?.Stop();
        public void Stop(StopBehaviour stopBehaviour) => PlaybackController?.Stop(stopBehaviour);
        public void MoveToTime(float time) =>PlaybackController?.MoveToTime(time);
        public float GetProgress(SequenceStep step) => PlaybackController?.GetProgress(step) ?? 0f;
        #if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => PlaybackController?.PlayAndWait() ?? UniTask.CompletedTask;
        #endif

        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<SequenceStep> steps = new();
        [SerializeField] private SequenceSettings settings;

        private SequencePlaybackController playbackController;
        private bool destroyed;

        private SequencePlaybackController PlaybackController
        {
            get
            {
                if(playbackController == null && !destroyed) 
                {
                    playbackController = SequencePlaybackController.Create(storage, settings, steps);
                }
                return playbackController;
            }
        }
        
        public void AddStep(SequenceStep step) => steps.Add(step);

        public void CopyFrom(SequenceDefinitionAsset sequenceDefinition)
        {
            Clear();
            storage = sequenceDefinition.Storage.Clone();
            settings = sequenceDefinition.Settings.Clone();
            foreach (var step in sequenceDefinition.Steps) steps.Add(step.Clone());
        }

        public void Clear()
        {
            steps.Clear();
            storage.Clear();
        }

        protected virtual void Update()
        {
            var dt = settings.Timescale switch
            {
                TimescaleMode.Normal => Time.deltaTime,
                TimescaleMode.Unscaled => Time.unscaledDeltaTime,
                _ => throw new ArgumentOutOfRangeException()
            };
            PlaybackController.UpdateTime(dt);
        }

        protected virtual void Awake()
        {
            if(settings.AutoPlay == AutoPlayMode.Awake) PlaybackController.Play();
        }

        protected virtual void Start()
        {
            if(settings.AutoPlay == AutoPlayMode.Start) PlaybackController.Play();
        }

        protected virtual void OnEnable()
        {
            if (settings.AutoPlay == AutoPlayMode.OnEnable) PlaybackController.Play();
        }

        protected virtual void OnDisable()
        {
            if (!Application.isPlaying) PlaybackController.Stop();
        }

        private void OnDestroy()
        {
            destroyed = true;
            PlaybackController.Release();
            playbackController = null;
        }

        private void UpdateTime(float dt) => PlaybackController?.UpdateTime(dt);

    }
}