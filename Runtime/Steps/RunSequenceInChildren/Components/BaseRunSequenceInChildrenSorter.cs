using System;
using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class BaseRunSequenceInChildrenSorter
    {
        public abstract void Sort(VarioStorage storage, List<ISequencePlayer> sequences);
    }
}