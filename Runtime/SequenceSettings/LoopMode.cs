namespace Damdor.Sequentio
{
    /// <summary>
    /// Specifies how playback continues after reaching a sequence boundary.
    /// </summary>
    public enum LoopMode
    {
        /// <summary>
        /// Plays once and finishes at the boundary.
        /// </summary>
        None,
       
        /// <summary>
        /// Wraps time to the opposite boundary and continues.
        /// </summary>
        Loop,
       
        /// <summary>
        /// Reverses direction at each boundary.
        /// </summary>
        PingPong
    }
}