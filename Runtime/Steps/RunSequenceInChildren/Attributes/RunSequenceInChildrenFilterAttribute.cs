using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RunSequenceInChildrenFilterAttribute : Attribute
    {
        public string Name { get; }
            
        public RunSequenceInChildrenFilterAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeRunSequenceInChildrenFilterSequenceParameter : SerializeFieldSequenceParameter
    {
        public override SerializedStepComponent GetSettings()
        {
            return RunSequenceInChildrenSettings.Filters;
        }
    }
}