using System;
using System.Collections.Generic;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    
    public class RawSequence : ISequence, IVarioStorageSource
    {
        public VarioStorage Storage => storage;
        public SequenceState State => state;
        public float Duration => runner?.Duration ?? 0f;
        public float CurrentTime => runner?.CurrentTime ?? 0f;
        private SequenceRunner runner
        {
            get
            {
                if (innerRunner != null || !isSetup) return innerRunner;
                innerRunner = SequentioPooling.PopRunner();
                SequentioUpdateProvider.Register(Update);
                return innerRunner;
            }
        }

        private VarioStorage storage;
        private List<SequenceStep> steps = new();
        private SequenceSettings settings;
        private bool autoRelease;

        private SequenceState state;
        private SequenceRunner innerRunner;
        private bool isSetup;

        internal RawSequence() { }

        public static RawSequence Create(VarioStorage storage = null, List<SequenceStep> steps = null, SequenceSettings settings = null)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.storage = storage != null ? storage.Clone() : VarioStorage.Create();
            if (steps != null)
            {
                foreach (var step in steps) sequence.steps.Add(step.Clone());
            }
            sequence.settings = settings != null ? settings.Clone() : SequentioPooling.PopSettings();
            sequence.isSetup = true;
            return sequence;
        }

        public void Release()
        {
            SequentioPooling.ReleaseRawSequence(this);
        }

        internal void Reset()
        {
            isSetup = false;
            storage.Release();
            storage = null;
            state = SequenceState.Idle;
            SequentioPooling.ReleaseSettings(settings);
            if (steps != null)
            {
                foreach (var step in steps) SequentioPooling.ReleaseStep(step);
                steps.Clear();
            }

            if (innerRunner != null)
            {
                SequentioPooling.ReleaseRunner(innerRunner);
                innerRunner = null;
            }

            if (settings != null)
            {
                SequentioPooling.ReleaseSettings(settings);
                settings = null;
            }
            
            autoRelease = false;
            SequentioUpdateProvider.Unregister(Update);
        }
        
        public void Init()
        {
            Play();
            Pause();
        }

        public void AutoRelease()
        {
            autoRelease = true;
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

        private void Update(float deltaTime, float unscaledDeltaTime)
        {
            var dt = settings.Timescale switch
            {
                TimescaleMode.Normal => deltaTime,
                TimescaleMode.Unscaled => unscaledDeltaTime,
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
            if (result == SequenceRunnerUpdateResult.Finished)
            {
                state = SequenceState.Finished;
                if (autoRelease) Release();
            }
        }
        
    }
}