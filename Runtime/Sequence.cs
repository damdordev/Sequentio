using System;
using System.Collections.Generic;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    
    public class Sequence : MonoBehaviour, ISequence, IVarioStorageSource
    {
        public VarioStorage Storage => storage;
        public SequenceState State => state;
        public float Duration => runner.Duration;
        public float CurrentTime => runner.CurrentTime;
        private SequenceRunner runner
        {
            get
            {
                if (innerRunner != null) return innerRunner;
                innerRunner = SequentioPooling.PopRunner();
                return innerRunner;
            }
        }
        
        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<SequenceStep> steps = new();
        [SerializeField] private SequenceSettings settings;

        private SequenceState state;
        private SequenceRunner innerRunner;

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
                    runner.Setup(Application.isPlaying, storage, steps, settings.Loop, settings.Direction);
                    runner.Init();
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
        
        #if DAMDOR_SEQUENTIO_UNITASK

        public UniTask PlayAndWait()
        {
            Play();
            return UniTask.WaitUntil(() => State == SequenceState.Finished);
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
            if (state != SequenceState.Idle)
            {
                ReturnToInitialState();
            }
        }
        
        public void Release(){}

        public void AddStep(SequenceStep step) => steps.Add(step);

        public void MoveToTime(float time)
        {
            if (state == SequenceState.Idle) return;
            
            runner.SetTime(time);
        }

        private void ReturnToInitialState()
        {
            runner.ReturnToInitialState();
            state = SequenceState.Idle;
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
            if (innerRunner != null) SequentioPooling.ReleaseRunner(innerRunner);
            innerRunner = null;
        }

        private void UpdateTime(float dt)
        {
            if (state != SequenceState.Playing) return;
            var result = runner.Update(dt);
            if (result == SequenceRunnerUpdateResult.Finished) state = SequenceState.Finished;
        }
        
    }
}