// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequenceDefinition : ISequenceDefinition
    {
        public IReadOnlyList<StepDefinition> Steps => steps;
        public VarioStorage Storage => storage;

        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<StepDefinition> steps = new();

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