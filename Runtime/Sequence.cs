using System;
using System.Collections.Generic;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    
    public class Sequence : MonoBehaviour, ISequence, ISequenceAsset, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storage;
        public IReadOnlyList<SequenceStep> Steps => steps;
        
        public SequenceState State => stateManager?.State ?? SequenceState.Idle;
        public float Duration => stateManager?.Duration ?? 0f;
        public float CurrentTime => stateManager?.CurrentTime ?? 0f;

        public void Init() => stateManager?.Init();
        public void Play() => stateManager?.Play();
        public void Pause() => stateManager?.Pause();
        public void Stop() => stateManager?.Stop();
        public void Stop(StopBehaviour stopBehaviour) => stateManager?.Stop(stopBehaviour);
        public void MoveToTime(float time) =>stateManager?.MoveToTime(time);
        public float GetProgress(SequenceStep step) => stateManager?.GetProgress(step) ?? 0f;
        public UniTask PlayAndWait() => stateManager?.PlayAndWait() ?? UniTask.CompletedTask;

        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<SequenceStep> steps = new();
        [SerializeField] private SequenceSettings settings;

        private SequenceStateManager _stateManager;
        private bool destroyed;

        private SequenceStateManager stateManager
        {
            get
            {
                if(_stateManager == null && !destroyed) 
                {
                    _stateManager = SequenceStateManager.Create(storage, settings, steps);
                }
                return _stateManager;
            }
        }
        
        public void AddStep(SequenceStep step) => steps.Add(step);

        public void CopyFrom(SequenceAsset sequenceAsset)
        {
            Clear();
            storage = sequenceAsset.Storage.Clone();
            settings = sequenceAsset.Settings.Clone();
            foreach (var step in sequenceAsset.Steps) steps.Add(step.Clone());
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
            stateManager.UpdateTime(dt);
        }

        protected virtual void Awake()
        {
            if(settings.AutoPlay == AutoPlayMode.Awake) stateManager.Play();
        }

        protected virtual void Start()
        {
            if(settings.AutoPlay == AutoPlayMode.Start) stateManager.Play();
        }

        protected virtual void OnEnable()
        {
            if (settings.AutoPlay == AutoPlayMode.OnEnable) stateManager.Play();
        }

        protected virtual void OnDisable()
        {
            if (!Application.isPlaying) stateManager.Stop();
        }

        private void OnDestroy()
        {
            destroyed = true;
            stateManager.Release();
            _stateManager = null;
        }

        private void UpdateTime(float dt) => stateManager?.UpdateTime(dt);

    }
}