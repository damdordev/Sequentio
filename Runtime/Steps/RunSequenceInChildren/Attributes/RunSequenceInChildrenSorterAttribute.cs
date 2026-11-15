using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RunSequenceInChildrenSorterAttribute : Attribute
    {
        public string Name { get; set; }
            
        public RunSequenceInChildrenSorterAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeRunSequenceInChildrenSorterSequenceParameter : SerializeFieldSequenceParameter
    {
        public override SerializedStepComponent GetSettings()
        {
            return RunSequenceInChildrenSettings.Sorters;
        }
    }
}