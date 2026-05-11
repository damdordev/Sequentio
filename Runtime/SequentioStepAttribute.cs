using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SequentioStepAttribute : Attribute
    {
        public string Name { get; }

        public SequentioStepAttribute(string name)
        {
            Name = name;
        }
    }
}