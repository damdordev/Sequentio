namespace Damdor.Sequentio
{
    /// <summary>
    /// Specifies whether stopping restores the target values captured during initialization.
    /// </summary>
    public enum StopBehaviour
    {
        /// <summary>
        /// Restores target values captured during step initialization.
        /// </summary>
        ReturnToInitialState,
       
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Leaves targets at their current values.
        /// </summary>
        LeaveInCurrentState
    }
}