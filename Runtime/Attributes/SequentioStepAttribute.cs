using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SequentioStepAttribute : Attribute
    {
        public string Path { get; }
        public string Name { get; }
        public string Icon { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Type PlayerType { get; }

        public SequentioStepAttribute(Type playerType, string path, string name, string icon = null)
        {
            PlayerType = playerType;
            Path = path;
            Name = name;
            Icon = icon;
        }
    }
}