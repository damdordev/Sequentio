using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SequenceStepParameterGroupAttribute : Attribute
    {
        public string ParameterGroup { get; }
        public int PriorityInGroup { get; }

        public SequenceStepParameterGroupAttribute(
            string parameterGroup,
            int priorityInGroup = 0)
        {
            ParameterGroup = parameterGroup;
            PriorityInGroup = priorityInGroup;
        }
    }
}