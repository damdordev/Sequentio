using System;

namespace Damdor.Sequentio
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class SerializeFieldSequenceParameter : Attribute
    {
        public abstract SerializedStepComponent GetSettings();
    }
}