using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Assigns a serialized step field to an editor group and controls its ordering within that group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SequenceStepParameterGroupAttribute : Attribute
    {
        /// <summary>
        /// The group name displayed in the step inspector.
        /// </summary>
        public string ParameterGroup { get; }
        /// <summary>
        /// The field ordering priority within its group; lower values appear first.
        /// </summary>
        public int PriorityInGroup { get; }

        /// <summary>
        /// Creates an inspector group assignment and ordering priority.
        /// </summary>
        /// <param name="parameterGroup">The name of the inspector group containing the field.</param>
        /// <param name="priorityInGroup">The ordering priority within the group; lower values appear first.</param>
        public SequenceStepParameterGroupAttribute(
            string parameterGroup,
            int priorityInGroup = 0)
        {
            ParameterGroup = parameterGroup;
            PriorityInGroup = priorityInGroup;
        }
    }
}