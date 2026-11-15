using System;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class BaseRunSequenceInChildrenFilter
    {
        public abstract bool Check(VarioStorage storage, ISequencePlayer sequence);
    }
}