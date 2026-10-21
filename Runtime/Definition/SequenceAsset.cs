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
        public SequenceSettings Settings => definition.Settings;
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
        public VarioStorage Storage => definition.Storage;
        public SequencePlan CreateDefinitionPlan() => definition.CreateDefinitionPlan();

        [SerializeField] private SequenceDefinition definition;
    }
}