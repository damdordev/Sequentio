using System;
using System.Collections.Generic;
using Damdor.Vario;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
#endif

namespace Damdor.Sequentio
{
    /// <summary>
    /// Provides pooled sequence playback independent of a Unity component. Release the instance when it is no longer needed.
    /// </summary>
    public class RuntimeSequence : ISequenceDefinition, ISequencePlayer, ISequentioUpdateReceiver
    {
        /// <summary>
        /// The current playback state.
        /// </summary>
        public SequencePlayerState State { get; private set; }
        
        /// <summary>
        /// The playback settings for this sequence.
        /// </summary>
        public SequenceSettings Settings => settings;
        
        /// <summary>
        /// The ordered steps in this sequence.
        /// </summary>
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
        
        /// <summary>
        /// The variable storage associated with this sequence.
        /// </summary>
        public VarioStorage Storage => definition.Storage;

        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        public float Duration
        {
            get
            {
                if(execution != null) return execution.Duration;
                var definitionPlan = SequenceCompiler.CreateDefinitionPlan(definition, settings, modify);
                var duration = definitionPlan.Duration;
                definitionPlan.Release();
                return duration;
            }
        }

        /// <summary>
        /// The current position on the sequence timeline, in seconds.
        /// </summary>
        public float CurrentTime => execution?.CurrentTime ?? 0;

        private SequenceDefinition ownedDefinition;
        private ISequenceDefinition definition;
        private SequenceSettings settings;
        private SequenceExecutor runner;
        private Action<VarioStorage, SequenceSettings> modify;
        private SequenceExecution execution;

        /// <summary>
        /// Obtains a pooled runtime sequence using the supplied definition and cloned settings.
        /// </summary>
        /// <param name="definition">The definition used to configure playback.</param>
        /// <param name="settings">The playback settings to copy for the sequence.</param>
        /// <param name="modify">An optional callback that modifies cloned storage and settings before step timings are evaluated.</param>
        /// <returns>A pooled instance. Call its Release method when it is no longer needed.</returns>
        public static RuntimeSequence Create(ISequenceDefinition definition, SequenceSettings settings, Action<VarioStorage, SequenceSettings> modify = null)
        {
            var runtimeSequence = SequentioPooling.PopSequenceRuntimeSequence();
            runtimeSequence.Setup(definition, settings.Clone(), modify);
            return runtimeSequence;
        }

        /// <summary>
        /// Creates a pooled runtime sequence for playback using the specified sequence definition.
        /// </summary>
        /// <param name="definition">The sequence definition that provides the steps and storage for playback.</param>
        /// <returns>A pooled runtime sequence instance. Release it when it is no longer needed.</returns>
        public static RuntimeSequence Create(ISequenceDefinition definition)
        {
            var runtimeSequence = SequentioPooling.PopSequenceRuntimeSequence();
            runtimeSequence.Setup(definition, SequentioPooling.PopSequenceSettings(), null);
            return runtimeSequence;
        }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Obtains a pooled runtime sequence with an empty owned definition and default settings.
        /// </summary>
        /// <returns>A pooled instance. Call its Release method when it is no longer needed.</returns>
        public static RuntimeSequence Create()
        {
            var sequencePlaybackPlayer = SequentioPooling.PopSequenceRuntimeSequence();
            sequencePlaybackPlayer.SetupNew();
            return sequencePlaybackPlayer;
        }
        
        private void Setup(ISequenceDefinition definition, SequenceSettings settings, Action<VarioStorage, SequenceSettings> modify = null)
        {
            ReleaseOwnedDefinition();
            this.definition = definition;
            this.modify = modify;
            this.settings = settings;
        }

        private void SetupNew()
        {
            ReleaseOwnedDefinition();
            ownedDefinition = SequentioPooling.PopSequenceDefinition();
            settings = SequentioPooling.PopSequenceSettings();
            definition = ownedDefinition;
            modify = null;
        }

        /// <summary>
        /// Starts playback to create the execution, then pauses it for manual control.
        /// </summary>
        public void Init()
        {
            Play();
            Pause();
        }

        /// <summary>
        /// Starts playback, resumes a paused sequence, or restarts a finished sequence.
        /// </summary>
        public void Play()
        {
            switch (State)
            {
                case SequencePlayerState.Idle:
                    var definitionPlan = SequenceCompiler.CreateDefinitionPlan(definition, settings, modify);
                    execution = SequenceCompiler.CreateExecution(definitionPlan);
                    definitionPlan.Release();
                    runner = SequenceExecutor.Create(execution);
                    State = SequencePlayerState.Playing;
                    SequentioUpdateProvider.Register(this, execution.Settings.Timescale == TimescaleMode.Unscaled);
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
        /// <summary>
        /// Starts playback and waits until the sequence finishes or returns to the idle state.
        /// </summary>
        /// <returns>A task that completes when playback finishes or is stopped.</returns>
        public UniTask PlayAndWait()
        {
            Play();
            return UniTask.WaitUntil(() => State is SequencePlayerState.Finished or SequencePlayerState.Idle);
        }
#endif
        
        /// <summary>
        /// Pauses playback if the sequence is currently playing.
        /// </summary>
        public void Pause()
        {
            if (State == SequencePlayerState.Playing)
            {
                State = SequencePlayerState.Paused;
            }
        }
        
        /// <summary>
        /// Stops playback using the configured default stop behavior.
        /// </summary>
        public void Stop()
        {
            if (State == SequencePlayerState.Idle) return;
            Stop(execution.Settings.DefaultStopBehaviour);
        }
        
        /// <summary>
        /// Stops playback using the specified target restoration behavior.
        /// </summary>
        /// <param name="stopBehaviour">Whether to restore captured initial target values or retain current values.</param>
        public void Stop(StopBehaviour stopBehaviour)
        {
            if (State == SequencePlayerState.Idle) return;
            State = SequencePlayerState.Idle;
            if(stopBehaviour == StopBehaviour.ReturnToInitialState) runner.ReturnToInitialState();
            ReleaseSequencePlanRunner();
        }
        
        /// <summary>
        /// Applies the requested sequence time. Has no effect while the player is idle.
        /// </summary>
        /// <param name="time">The requested sequence time in seconds, clamped to the sequence duration.</param>
        public void MoveToTime(float time)
        {
            if (State == SequencePlayerState.Idle) return;
            
            runner.SetTime(time);
        }

        /// <summary>
        /// Advances sequence playback by the supplied elapsed time.
        /// </summary>
        /// <param name="dt">The elapsed time in seconds before applying playback speed and direction.</param>
        public void Update(float dt)
        {
            if (State != SequencePlayerState.Playing) return;
            var result = runner.Update(dt);
            if (result == SequenceExecutorUpdateResult.Finished) State = SequencePlayerState.Finished;
        }
        
        public T AddStep<T>() where T : StepDefinition, new() => definition.AddStep<T>();
        public void RemoveStep(int index) => definition.RemoveStep(index);
        public T InsertStep<T>(int index) where T : StepDefinition, new() => definition.InsertStep<T>(index);
        
        private void ReleaseSequencePlanRunner()
        {
            if (runner == null) return;
            runner.Release();
            runner = null;
            execution?.Release();
            execution = null;
            SequentioUpdateProvider.Unregister(this);
        }

        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
        public void Release()
        {
            SequentioPooling.ReleaseRuntimeSequence(this);
        }

        /// <summary>
        /// Gets the last assigned normalized progress of the specified step.
        /// </summary>
        /// <param name="definition">The step definition whose progress or metadata is requested.</param>
        /// <returns>The step progress, or zero if there is no active execution or the step is not present.</returns>
        public float GetProgress(StepDefinition definition)
        {
            if (State == SequencePlayerState.Idle) return 0f;
            foreach (var stepPlayerPlan in execution.Steps)
            {
                if(stepPlayerPlan.Definition == definition) return stepPlayerPlan.Progress;
            }

            return 0f;
        }
        
        internal void Reset()
        {
            State = SequencePlayerState.Idle;
            definition = null;
            modify = null;
            settings?.Release();
            settings = null;
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
