namespace Damdor.Sequentio
{
    /// <summary>
    /// Specifies which Unity lifecycle callback starts playback automatically.
    /// </summary>
    public enum AutoPlayMode
    {
        /// <summary>
        /// Disables automatic playback.
        /// </summary>
        None,
       
        /// <summary>
        /// Starts playback in Awake.
        /// </summary>
        Awake,
        
        /// <summary>
        /// Starts playback in Start.
        /// </summary>
        Start,
        
        /// <summary>
        /// Starts playback when the component is enabled.
        /// </summary>
        OnEnable
    }
}