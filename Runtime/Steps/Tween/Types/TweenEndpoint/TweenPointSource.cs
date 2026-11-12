namespace Damdor.Sequentio
{
    /// <summary>
    /// Selects how a tween endpoint is evaluated.
    /// </summary>
    public enum TweenPointSource
    {
        /// <summary>
        /// Uses the configured value directly.
        /// </summary>
        ExplicitValue,
        /// <summary>
        /// Adds the configured offset to the target value at initialization.
        /// </summary>
        CurrentValue,
        /// <summary>
        /// Adds the configured offset to the referenced target value at initialization.
        /// </summary>
        ReferenceTarget
    }
}