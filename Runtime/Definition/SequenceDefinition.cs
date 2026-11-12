// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Stores an ordered collection of step definitions and their shared variables.
    /// </summary>
    [Serializable]
    public class SequenceDefinition : ISequenceDefinition
    {
        /// <summary>
        /// The ordered steps in this sequence.
        /// </summary>
        public IReadOnlyList<StepDefinition> Steps => steps;
       
        /// <summary>
        /// The variable storage associated with this sequence.
        /// </summary>
        public VarioStorage Storage => storage;

        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<StepDefinition> steps = new();

        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
        public void Release()
        {
            SequentioPooling.ReleaseSequenceDefinition(this);
        }
        
        internal void Reset()
        {
            steps.Clear();
            storage.Clear();
        }
    }
}