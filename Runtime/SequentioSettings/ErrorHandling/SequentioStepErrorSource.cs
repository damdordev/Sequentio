namespace Damdor.Sequentio
{
    /// <summary>
    /// Identifies the operation that raised a step error.
    /// </summary>
    public enum SequentioStepErrorSource
    {
        /// <summary>
        /// An error occurred while evaluating step duration.
        /// </summary>
        CalculateDuration,
        /// <summary>
        /// An error occurred while creating or configuring a step player.
        /// </summary>
        CreatePlayer,
        /// <summary>
        /// An error occurred while initializing a step player.
        /// </summary>
        InitPlayer,
        /// <summary>
        /// An error occurred while applying step progress.
        /// </summary>
        SetProgress,
        /// <summary>
        /// An error occurred while restoring initial target values.
        /// </summary>
        ReturnToInitialState
    }
}