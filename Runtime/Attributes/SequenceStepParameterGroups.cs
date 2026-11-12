namespace Damdor.Sequentio
{
    // ReSharper disable once PartialTypeWithSinglePart
    /// <summary>
    /// Defines standard group names for step parameters in the inspector.
    /// </summary>
    public static partial class SequenceStepParameterGroups
    {
        /// <summary>
        /// The group name for common step options.
        /// </summary>
        public const string General = "General";
        /// <summary>
        /// The group name for motion parameters.
        /// </summary>
        public const string Motion = "Motion";
        /// <summary>
        /// The fallback group name for fields without an explicit group.
        /// </summary>
        public const string Default = "Other";
    }
}