using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class BaseRunSequenceInChildrenGetter
    {
        public abstract void GetSequences(VarioStorage storage, GameObject root, List<ISequencePlayer> sequences);
    }
}