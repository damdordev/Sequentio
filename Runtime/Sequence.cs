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
        
        public SequenceState State => state;
        public float Duration => runner?.Duration ?? 0f;
        public float CurrentTime => runner?.CurrentTime ?? 0f;
        
        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<SequenceStep> steps = new();
        [SerializeField] private SequenceSettings settings;

        private SequenceState state;
        private SequenceRunner runner;

        public void Init()
        {
            Play();
            Pause();
        }
        
        public void Play()
        {
            switch (state)
            {
                case SequenceState.Idle:
                    runner = SequentioPooling.PopRunner();
                    runner.Setup(Application.isPlaying, storage, steps, settings);
                    state = SequenceState.Playing;
                    break;
                case SequenceState.Playing:
                    break;
                case SequenceState.Paused:
                    // resume, should I have separate method Resume?
                    state = SequenceState.Playing;
                    break;
                case SequenceState.Finished:
                    Stop();
                    Play();
                    break;
            }
        }
        
        public float GetProgress(SequenceStep step) => runner?.GetProgress(step) ?? 0f;
        
        #if DAMDOR_SEQUENTIO_UNITASK

        public UniTask PlayAndWait()
        {
            Play();
            return UniTask.WaitUntil(() => State == SequenceState.Finished || State == SequenceState.Idle);
        }
        
        #endif

        public void Pause()
        {
            if (state == SequenceState.Playing)
            {
                state = SequenceState.Paused;
            }
        }

        public void Stop()
        {
            Stop(Settings.DefaultStopBehaviour);
        }

        public void Stop(StopBehaviour stopBehaviour)
        {
            if (state == SequenceState.Idle) return;
            state = SequenceState.Idle;
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) runner?.ReturnToInitialState();
            runner?.Release();
            ReleaseRunner();
        }
        
        public void Release(){}

        public void AddStep(SequenceStep step) => steps.Add(step);

        public void MoveToTime(float time)
        {
            if (state == SequenceState.Idle) return;
            
            runner.SetTime(time);
        }

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
            UpdateTime(dt);
        }

        protected virtual void Awake()
        {
            if(settings.AutoPlay == AutoPlayMode.Awake) Play();
        }

        protected virtual void Start()
        {
            if(settings.AutoPlay == AutoPlayMode.Start) Play();
        }

        protected virtual void OnEnable()
        {
            if (settings.AutoPlay == AutoPlayMode.OnEnable)
            {
                Stop();
                Play();
            }
        }

        protected void OnDisable()
        {
            if (!Application.isPlaying) Stop();
        }

        private void OnDestroy()
        {
            ReleaseRunner();
        }

        private void UpdateTime(float dt)
        {
            if (state != SequenceState.Playing) return;
            var result = runner.Update(dt);
            if (result == SequenceRunnerUpdateResult.Finished) state = SequenceState.Finished;
        }

        private void ReleaseRunner()
        {
            if (runner == null) return;
            runner.Release();
            runner = null;
        }
        
    }
}