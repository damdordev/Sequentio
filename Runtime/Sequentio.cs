using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public enum SequentioState
    {
        Idle,
        Playing,
        Paused,
        Finished
    }
    
    public class Sequentio : MonoBehaviour, IVarioStorageSource
    {
        public VarioStorage Storage => storage;
        
        [SerializeField] private VarioStorage storage;
        [SerializeReference] private List<SequentioStep> steps;

        private List<SequentioStep> currentSteps;
        private SequentioState state;
        private SequentioRunner runner;

        public void Init()
        {
            Play();
            Pause();
        }
        
        public void Play()
        {
            switch (state)
            {
                case SequentioState.Idle:
                    runner = new SequentioRunner(Application.isPlaying, storage, steps);
                    runner.Init();
                    break;
                case SequentioState.Playing:
                    break;
                case SequentioState.Paused:
                    // resume, should I have separate method Resume?
                    state = SequentioState.Playing;
                    break;
                case SequentioState.Finished:
                    Stop();
                    Play();
                    break;
            }
        }

        public UniTask PlayAndWait()
        {
            return UniTask.CompletedTask;
        }

        public void Pause()
        {
            if (state == SequentioState.Playing)
            {
                state = SequentioState.Paused;
            }
        }

        public void Stop()
        {
            if (state != SequentioState.Idle)
            {
                ReturnToInitialState();
            }
        }

        private void ReturnToInitialState()
        {
            runner.ReturnToInitialState();
            state = SequentioState.Idle;
            runner = null;
        }
        
    }
}