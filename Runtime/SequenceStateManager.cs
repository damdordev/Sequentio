#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceStateManager
    {
        public SequenceState State => state;
        public float Duration => runner?.Duration ?? 0f;
        public float CurrentTime => runner?.CurrentTime ?? 0f;
        
        private VarioStorage storage;
        private List<SequenceStep> steps;
        private SequenceSettings settings;
        
        private SequenceState state;
        private SequenceRunner runner;

        public static SequenceStateManager Create(VarioStorage storage, SequenceSettings settings, List<SequenceStep> steps)
        {
            var manager = SequentioPooling.PopSequenceStateManager();
            manager.Setup(storage, settings, steps);
            return manager;
        }
        
        public void Setup(VarioStorage storage, SequenceSettings settings, List<SequenceStep> steps)
        {
            this.storage = storage;
            this.settings = settings;
            this.steps = steps;
        }

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
            Stop(settings.DefaultStopBehaviour);
        }

        public void Stop(StopBehaviour stopBehaviour)
        {
            if (state == SequenceState.Idle) return;
            state = SequenceState.Idle;
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) runner?.ReturnToInitialState();
            ReleaseRunner();
        }
        
        public void MoveToTime(float time)
        {
            if (state == SequenceState.Idle) return;
            
            runner.SetTime(time);
        }
        
        public void UpdateTime(float dt)
        {
            if (state != SequenceState.Playing) return;
            var result = runner.Update(dt);
            if (result == SequenceRunnerUpdateResult.Finished) state = SequenceState.Finished;
        }
        
        public float GetProgress(SequenceStep step) => runner?.GetProgress(step) ?? 0f;

        public void Release()
        {
            
        }

        internal void Reset()
        {
            storage = null;
            steps = null;
            settings = null;

            state = SequenceState.Idle;
            runner?.Release();
            runner = null;
        }

        private void ReleaseRunner()
        {
            if (runner == null) return;
            runner.Release();
            runner = null;
        }
        
    }
}