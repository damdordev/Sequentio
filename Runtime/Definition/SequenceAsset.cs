// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [CreateAssetMenu(fileName = "sequence", menuName = "Damdor/Sequntio/Sequence asset")]
    public class SequenceAsset : ScriptableObject, ISequenceDefinition, IVarioStorageSource
    {
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
        public VarioStorage Storage => definition.Storage;

        [SerializeField] private SequenceDefinition definition;
    }
}