#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global
#endif

namespace Damdor.Sequentio
{
    /// <summary>
    /// Controls sequence playback and exposes its timing and state.
    /// </summary>
    public interface ISequencePlayer
    {
        /// <summary>
        /// The current playback state.
        /// </summary>
        SequencePlayerState State { get; }
        
        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        float Duration { get; }
        
        /// <summary>
        /// The current position on the sequence timeline, in seconds.
        /// </summary>
        float CurrentTime { get; }
        
        /// <summary>
        /// Starts playback to create the execution, then pauses it. It should be used to
        /// initialize the scene according to the sequence's initial state. 
        /// </summary>
        void Init();
        
        /// <summary>
        /// Starts playback, resumes a paused sequence, or restarts a finished sequence.
        /// </summary>
        void Play();
        
        /// <summary>
        /// Pauses playback if the sequence is currently playing.
        /// </summary>
        void Pause();
        
        /// <summary>
        /// Stops playback using the configured default stop behavior.
        /// </summary>
        void Stop();
       
        /// <summary>
        /// Stops playback using the specified target restoration behavior.
        /// </summary>
        /// <param name="stopBehaviour">Whether to restore captured initial target values or retain current values.</param>
        void Stop(StopBehaviour stopBehaviour);
        
        /// <summary>
        /// Applies the requested sequence time. Has no effect while the player is idle.
        /// </summary>
        /// <param name="time">The requested sequence time in seconds, clamped to the sequence duration.</param>
        void MoveToTime(float time);
        
        /// <summary>
        /// Gets the last assigned normalized progress of the specified step.
        /// </summary>
        /// <param name="step">The step definition whose progress or metadata is requested.</param>
        /// <returns>The step progress, or zero if there is no active execution or the step is not present.</returns>
        float GetProgress(StepDefinition step);

#if DAMDOR_SEQUENTIO_UNITASK
        /// <summary>
        /// Starts playback and waits until the sequence finishes or returns to the idle state.
        /// </summary>
        /// <returns>A task that completes when playback finishes or is stopped.</returns>
        UniTask PlayAndWait();
#endif
    }
}