
using System;
using System.Collections.Generic;
using Damdor.Vario;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
#endif

namespace Damdor.Sequentio
{
    public class RuntimeSequence : ISequenceDefinition, ISequencePlayer
    {
        public SequencePlayerState State { get; private set; }
        public SequenceExecution Execution { get; private set; }
        public SequenceSettings Settings => definition.Settings;
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
        public VarioStorage Storage => definition.Storage;
        public SequencePlan CreateDefinitionPlan() => definition.CreateDefinitionPlan();

        public float Duration
        {
            get
            {
                if(Execution != null) return Execution.Duration;
                var definitionPlan = SequenceCompiler.CreateDefinitionPlan(definition);
                var duration = definitionPlan.Duration;
                definitionPlan.Release();
                return duration;
            }
        }

        public float CurrentTime => Execution?.CurrentTime ?? 0;

        private SequenceDefinition ownedDefinition;
        private ISequenceDefinition definition;
        private SequenceExecutor runner;

        public static RuntimeSequence Create(ISequenceDefinition definition)
        {
            var sequencePlaybackPlayer = SequentioPooling.PopSequencePlaybackRunner();
            sequencePlaybackPlayer.Setup(definition);
            return sequencePlaybackPlayer;
        }

        // ReSharper disable once UnusedMember.Global
        public static RuntimeSequence Create()
        {
            var sequencePlaybackPlayer = SequentioPooling.PopSequencePlaybackRunner();
            sequencePlaybackPlayer.SetupNew();
            return sequencePlaybackPlayer;
        }
        
        private void Setup(ISequenceDefinition definition)
        {
            ReleaseOwnedDefinition();
            this.definition = definition;
        }

        private void SetupNew()
        {
            ReleaseOwnedDefinition();
            ownedDefinition = SequentioPooling.PopSequenceDefinition();
            definition = ownedDefinition;
        }

        public void Init()
        {
            Play();
            Pause();
        }

        public void Play()
        {
            switch (State)
            {
                case SequencePlayerState.Idle:
                    var definitionPlan = SequenceCompiler.CreateDefinitionPlan(definition);
                    Execution = SequenceCompiler.CreatePlayerPlan(definitionPlan);
                    definitionPlan.Release();
                    runner = SequenceExecutor.Create(Execution);
                    State = SequencePlayerState.Playing;
                    SequentioUpdateProvider.Register(Update, Execution.Settings.Timescale == TimescaleMode.Unscaled);
                    break;
                case SequencePlayerState.Playing:
                    break;
                case SequencePlayerState.Paused:
                    State = SequencePlayerState.Playing;
                    break;
                case SequencePlayerState.Finished:
                    Stop();
                    // ReSharper disable once TailRecursiveCall
                    Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
#if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait()
        {
            Play();
            return UniTask.WaitUntil(() => State is SequencePlayerState.Finished or SequencePlayerState.Idle);
        }
#endif
        
        public void Pause()
        {
            if (State == SequencePlayerState.Playing)
            {
                State = SequencePlayerState.Paused;
            }
        }
        
        public void Stop()
        {
            if (State == SequencePlayerState.Idle) return;
            Stop(Execution.Settings.DefaultStopBehaviour);
        }
        
        public void Stop(StopBehaviour stopBehaviour)
        {
            if (State == SequencePlayerState.Idle) return;
            State = SequencePlayerState.Idle;
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) runner.ReturnToInitialState();
            ReleaseSequencePlanRunner();
        }
        
        public void MoveToTime(float time)
        {
            if (State == SequencePlayerState.Idle) return;
            
            runner.SetTime(time);
        }

        private void Update(float dt)
        {
            if (State != SequencePlayerState.Playing) return;
            var result = runner.Update(dt);
            if (result == SequenceExecutorUpdateResult.Finished) State = SequencePlayerState.Finished;
        }
        
        private void ReleaseSequencePlanRunner()
        {
            if (runner == null) return;
            runner.ReleaseSteps();
            runner.Release();
            runner = null;
            Execution?.Release();
            Execution = null;
            SequentioUpdateProvider.Unregister(Update);
        }

        public void Release()
        {
            SequentioPooling.ReleaseSequencePlaybackRunner(this);
        }

        public float GetProgress(StepDefinition definition)
        {
            if (State == SequencePlayerState.Idle) return 0f;
            foreach (var stepPlayerPlan in Execution.Steps)
            {
                if(stepPlayerPlan.Definition == definition) return stepPlayerPlan.Progress;
            }

            return 0f;
        }
        
        internal void Reset()
        {
            State = SequencePlayerState.Idle;
            definition = null;
            ReleaseSequencePlanRunner();
            ReleaseOwnedDefinition();
        }

        private void ReleaseOwnedDefinition()
        {
            ownedDefinition?.Release();
            ownedDefinition = null;
        }
        
    }
}