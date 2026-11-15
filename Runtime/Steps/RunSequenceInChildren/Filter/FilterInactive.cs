using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenFilter("Remove inactive")]
    public class FilterInactive : BaseRunSequenceInChildrenFilter
    {
        public override bool Check(VarioStorage storage, ISequencePlayer sequence)
        {
            var component = sequence as Component;
            return component == null || component.gameObject.activeSelf;
        }
    }
}