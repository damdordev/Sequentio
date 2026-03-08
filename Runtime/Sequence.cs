using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public enum SequenceState
    {
        Idle,
        Playing,
        Paused,
        Finished
    }
    
    public class Sequence : MonoBehaviour, IVarioStorageSource
    {
        public VarioStorage Storage => storage;
        public SequenceState State => state;
        
        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] public List<SequentioStep> steps = new();

        private List<SequentioStep> currentSteps;
        private SequenceState state;
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
                case SequenceState.Idle:
                    runner = new SequentioRunner(Application.isPlaying, storage, steps);
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

        public void AddStep(SequentioStep step) => steps.Add(step);

        private void ReturnToInitialState()
        {
            runner.ReturnToInitialState();
            state = SequenceState.Idle;
            runner = null;
        }

        protected virtual void Update()
        {
            if (state == SequenceState.Playing) runner.Update(Time.deltaTime);
        }
        
    }
}