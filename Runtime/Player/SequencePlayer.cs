using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
#endif

namespace Damdor.Sequentio
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    /// <summary>
    /// Plays a sequence defined on this Unity component.
    /// </summary>
    public class SequencePlayer : MonoBehaviour, ISequenceDefinition, ISequencePlayer
    {
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
        /// The current playback state.
        /// </summary>
        public SequencePlayerState State => InnerSequence?.State ?? SequencePlayerState.Idle;
        
        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        public float Duration => InnerSequence?.Duration ?? 0f;
       
        /// <summary>
        /// The current position on the sequence timeline, in seconds.
        /// </summary>
        public float CurrentTime => InnerSequence?.CurrentTime ?? 0f;

        private RuntimeSequence innerSequence;
        private bool destroyed;

        [SerializeField] private SequenceSettings settings;
        [SerializeField] private SequenceDefinition definition;

        private RuntimeSequence InnerSequence
        {
            get
            {
                if (destroyed) return innerSequence;
                if (innerSequence == null) innerSequence = RuntimeSequence.Create(this, settings);
                else innerSequence.Settings.CopyFrom(settings);
                return innerSequence;
            }
        }

        /// Starts playback to create the execution, then pauses it. It should be used to
        /// initialize the scene according to the sequence's initial state. 
        public void Init() => InnerSequence?.Init();
        
        /// <summary>
        /// Starts playback, resumes a paused sequence, or restarts a finished sequence.
        /// </summary>
        public void Play() => InnerSequence?.Play();
        
        /// <summary>
        /// Pauses playback if the sequence is currently playing.
        /// </summary>
        public void Pause() => InnerSequence?.Pause();
        
        /// <summary>
        /// Stops playback using the configured default stop behavior.
        /// </summary>
        public void Stop() => InnerSequence?.Stop();
        
        /// <summary>
        /// Stops playback using the specified target restoration behavior.
        /// </summary>
        /// <param name="stopBehaviour">Whether to restore captured initial target values or retain current values.</param>
        public void Stop(StopBehaviour stopBehaviour) => InnerSequence?.Stop(stopBehaviour);
        
        /// <summary>
        /// Applies the requested sequence time. Has no effect while the player is idle.
        /// </summary>
        /// <param name="time">The requested sequence time in seconds, clamped to the sequence duration.</param>
        public void MoveToTime(float time) => InnerSequence?.MoveToTime(time);
        
        /// <summary>
        /// Gets the last assigned normalized progress of the specified step.
        /// </summary>
        /// <param name="step">The step definition whose progress or metadata is requested.</param>
        /// <returns>The step progress, or zero if there is no active execution or the step is not present.</returns>
        public float GetProgress(StepDefinition step) => innerSequence?.GetProgress(step) ?? 0f;

#if DAMDOR_SEQUENTIO_UNITASK
        /// <summary>
        /// Starts playback and waits until the sequence finishes or returns to the idle state.
        /// </summary>
        /// <returns>A task that completes when playback finishes or is stopped.</returns>
        public UniTask PlayAndWait() => InnerSequence?.PlayAndWait() ?? UniTask.CompletedTask;
#endif
        
        public T AddStep<T>() where T : StepDefinition, new() => definition.AddStep<T>();
        public void RemoveStep(int index) => definition.RemoveStep(index);
        public T InsertStep<T>(int index) where T : StepDefinition, new() => definition.InsertStep<T>(index);

        /// <summary>
        /// Starts playback when automatic playback is configured for Awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (Settings.AutoPlay == AutoPlayMode.Awake) InnerSequence?.Play();
        }

        /// <summary>
        /// Starts playback when automatic playback is configured for Start.
        /// </summary>
        protected virtual void Start()
        {
            if (Settings.AutoPlay == AutoPlayMode.Start) InnerSequence?.Play();
        }

        /// <summary>
        /// Starts playback when automatic playback is configured for OnEnable.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (Settings.AutoPlay == AutoPlayMode.OnEnable) InnerSequence?.Play();
        }

        /// <summary>
        /// Releases the runtime sequence when this component is disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            ReleaseRuntimeSequence();
        }
        
        /// <summary>
        /// Marks the component as destroyed and releases its runtime sequence.
        /// </summary>
        protected virtual void OnDestroy()
        {
            destroyed = true;
            ReleaseRuntimeSequence();
        }

        private void ReleaseRuntimeSequence()
        {
            InnerSequence?.Release();
            innerSequence = null;
        }
    }
}