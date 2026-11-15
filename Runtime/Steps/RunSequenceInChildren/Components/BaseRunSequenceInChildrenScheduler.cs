using System;
using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class BaseRunSequenceInChildrenScheduler
    {
        public abstract void Schedule(VarioStorage storage, List<ISequencePlayer> sequences, RunSequenceInChildrenSchedule target);
    }
}