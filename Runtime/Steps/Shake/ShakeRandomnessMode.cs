namespace Damdor.Sequentio
{
    /// <summary>
    /// Controls how random angular deviations are generated for a shake.
    /// </summary>
    public enum ShakeRandomnessMode
    {
        /// <summary>
        /// Uses positive and negative random angular deviations.
        /// </summary>
        Full,
        /// <summary>
        /// Uses nonnegative random angular deviations for a more regular shake.
        /// </summary>
        Harmonic,
    }
}