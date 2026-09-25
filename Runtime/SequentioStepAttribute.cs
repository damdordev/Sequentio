using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SequentioStepAttribute : Attribute
    {
        public string Path { get; }
        public string Name { get; }

        public SequentioStepAttribute(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }
}