using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RunSequenceInChildrenSchedulerAttribute : Attribute
    {
        public string Name { get; set; }
            
        public RunSequenceInChildrenSchedulerAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeRunSequenceInChildrenSchedulerSequenceParameter : SerializeFieldSequenceParameter
    {
        public override SerializedStepComponent GetSettings()
        {
            return RunSequenceInChildrenSettings.Schedulers;
        }
    }
}