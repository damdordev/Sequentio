using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RunSequenceInChildrenGetterAttribute : Attribute
    {
        public string Name { get; set; }
            
        public RunSequenceInChildrenGetterAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeRunSequenceInChildrenGetterSequenceParameter : SerializeFieldSequenceParameter
    {
        public override SerializedStepComponent GetSettings()
        {
            return RunSequenceInChildrenSettings.Getters;
        }
    }
}