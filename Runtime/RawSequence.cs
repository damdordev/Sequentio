using System;
using System.Collections.Generic;
using Damdor.Foundation;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    
    public class RawSequence : ISequence, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storage;
        public SequenceState State => state;
        public float Duration => runner?.Duration ?? 0f;
        public float CurrentTime => runner?.CurrentTime ?? 0f;

        private readonly List<SequenceStep> steps = new();
        private VarioStorage storage;
        private SequenceSettings settings;
        private bool autoRelease;

        private SequenceState state;
        private SequenceRunner runner;

        internal RawSequence() { }
        
        public static RawSequence Create(ISequenceAsset sequenceAsset)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.Setup(sequenceAsset);
            sequence.TryAutoPlay();
            return sequence;
        }
        
        public static RawSequence Create(ISequenceAsset sequenceAsset, UIDocument uiDocument)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.Setup(sequenceAsset);
            VarioHelper.PutRoot(sequence.Storage, uiDocument);
            sequence.TryAutoPlay();
            return sequence;
        }
        
        public static RawSequence Create(ISequenceAsset sequenceAsset, VisualElement root)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.Setup(sequenceAsset);
            VarioHelper.PutRoot(sequence.Storage, root);
            sequence.TryAutoPlay();
            return sequence;
        }
        
        public static RawSequence Create(ISequenceAsset sequenceAsset, UIDocument uiDocument, UiElementsQuery root)
        {
            var sequence = SequentioPooling.PopRawSequence();
            sequence.Setup(sequenceAsset);
            VarioHelper.PutRoot(sequence.Storage, uiDocument);
            VarioHelper.PutRoot(sequence.Storage, uiDocument, root);
            sequence.TryAutoPlay();
            return sequence;
        }

        public void Release()
        {
            Stop();
            SequentioPooling.ReleaseRawSequence(this);
        }
        
        internal void Reset()
        {
            storage.Release();
            storage = null;
            state = SequenceState.Idle;
            if (steps != null)
            {
                foreach (var step in steps) SequentioPooling.ReleaseStep(step);
                steps.Clear();
            }

            ReleaseRunner();

            if (settings != null)
            {
                SequentioPooling.ReleaseSettings(settings);
                settings = null;
            }
            
            autoRelease = false;
        }
        
        public void Init()
        {
            Play();
            Pause();
        }

        public void AutoRelease()
        {
            autoRelease = true;
            if(state == SequenceState.Finished) Release();
        }
        
        public void Play()
        {
            switch (state)
            {
                case SequenceState.Idle:
                    runner = new SequenceRunner();
                    runner.Setup(Application.isPlaying, storage, steps, settings);
                    state = SequenceState.Playing;
                    SequentioUpdateProvider.Register(Update);
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
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) runner?.ReturnToInitialState();
            ReleaseRunner();
            state = SequenceState.Idle;
        }

        public void AddStep(SequenceStep step) => steps.Add(step);

        public void MoveToTime(float time)
        {
            if (state == SequenceState.Idle) return;
            
            runner.SetTime(time);
        }
        
        private void Setup(ISequenceAsset sequenceAsset)
        {
            storage = sequenceAsset.Storage.Clone();
            settings = sequenceAsset.Settings.Clone();
            foreach (var step in sequenceAsset.Steps) steps.Add(step.Clone());
        }

        private void TryAutoPlay()
        {
            if (Settings.AutoPlay == AutoPlayMode.None) return;
            Play();
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
        
        private void ReleaseRunner()
        {
            if (runner == null) return;
            SequentioPooling.ReleaseRunner(runner);
            SequentioUpdateProvider.Unregister(Update);
            runner = null;
        }
        
    }
}