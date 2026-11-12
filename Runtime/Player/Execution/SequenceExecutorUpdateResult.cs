namespace Damdor.Sequentio
{
    /// <summary>
    /// Indicates whether an executor update completed playback.
    /// </summary>
    public enum SequenceExecutorUpdateResult
    {
        /// <summary>
        /// Playback can continue.
        /// </summary>
        NotFinished,
        
        /// <summary>
        /// Non-looping playback has reached its terminal boundary.
        /// </summary>
        Finished
    }
}