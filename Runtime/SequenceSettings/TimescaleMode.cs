namespace Damdor.Sequentio
{
    /// <summary>
    /// Selects scaled or unscaled elapsed time for playback.
    /// </summary>
    public enum TimescaleMode
    {
        /// <summary>
        /// Uses Unity scaled delta time in play mode.
        /// </summary>
        Normal,
        
        /// <summary>
        /// Uses Unity unscaled delta time in play mode.
        /// </summary>
        Unscaled
    }
}