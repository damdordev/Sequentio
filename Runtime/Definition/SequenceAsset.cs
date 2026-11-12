// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Stores a reusable sequence definition and its variables as a Unity asset.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "sequence", menuName = "Damdor/Sequntio/Sequence asset")]
    public class SequenceAsset : ScriptableObject, ISequenceDefinition, IVarioStorageSource
    {
        /// <summary>
        /// The ordered steps in this sequence.
        /// </summary>
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
      
        /// <summary>
        /// The variable storage associated with this sequence.
        /// </summary>
        public VarioStorage Storage => definition.Storage;

        [SerializeField] private SequenceDefinition definition;
    }
}