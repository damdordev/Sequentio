using System;
using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenSorter("Reverse")]
    public class ReverseOrder : BaseRunSequenceInChildrenSorter
    {
        public override void Sort(VarioStorage storage, List<ISequencePlayer> sequences)
        {
            sequences.Reverse();
        }
    }
}