using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SequentioStepAttribute : Attribute
    {
        public string Path { get; }
        public string Name { get; }
        public string Icon { get; }

        public SequentioStepAttribute(string path, string name, string icon = null)
        {
            Path = path;
            Name = name;
            Icon = icon;
        }
    }
}