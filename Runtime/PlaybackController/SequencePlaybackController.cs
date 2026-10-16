#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequencePlaybackController
    {
        public SequenceState State => state;
        public float Duration => executionPlayer?.Duration ?? 0f;
        public float CurrentTime => executionPlayer?.CurrentTime ?? 0f;
        
        private VarioStorage storage;
        private List<SequenceStep> steps;
        private SequenceSettings settings;
        
        private SequenceState state;
        private SequenceExecutionPlayer executionPlayer;

        #region Pooling

        public static SequencePlaybackController Create(VarioStorage storage, SequenceSettings settings, List<SequenceStep> steps)
        {
            var manager = SequentioPooling.PopSequencePlaybackController();
            manager.Setup(storage, settings, steps);
            return manager;
        }

        private void Setup(VarioStorage storage, SequenceSettings settings, List<SequenceStep> steps)
        {
            this.storage = storage;
            this.settings = settings;
            this.steps = steps;
        }

        public void Release()
        {
            SequentioPooling.ReleaseSequencePlaybackController(this);
        }
        
        internal void Reset()
        {
            storage = null;
            steps = null;
            settings = null;

            state = SequenceState.Idle;
            executionPlayer?.Release();
            executionPlayer = null;
        }

        #endregion
        
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
                    executionPlayer = SequenceExecutionPlayer.Create(storage, steps, settings, Application.isPlaying);
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
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) executionPlayer?.ReturnToInitialState();
            ReleaseExecutionPlayer();
        }
        
        public void MoveToTime(float time)
        {
            if (state == SequenceState.Idle) return;
            
            executionPlayer.SetTime(time);
        }
        
        public void UpdateTime(float dt)
        {
            if (state != SequenceState.Playing) return;
            var result = executionPlayer.Update(dt);
            if (result == SequenceExecutionUpdateResult.Finished) state = SequenceState.Finished;
        }
        
        public float GetProgress(SequenceStep step) => executionPlayer?.GetProgress(step) ?? 0f;

        private void ReleaseExecutionPlayer()
        {
            if (executionPlayer == null) return;
            executionPlayer.ReleaseSteps();
            executionPlayer.Release();
            executionPlayer = null;
        }
        
    }
}