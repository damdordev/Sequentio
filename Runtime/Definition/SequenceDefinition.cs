// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequenceDefinition : ISequenceDefinition, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public IReadOnlyList<StepDefinition> Steps => steps;
        public VarioStorage Storage => storage;
        public SequencePlan CreateDefinitionPlan() => SequenceCompiler.CreateDefinitionPlan(this, true, null);

        [SerializeField] private VarioStorage storage = new();
        [SerializeField] private SequenceSettings settings = new();
        [SerializeReference] private List<StepDefinition> steps = new();

        public void Release()
        {
            SequentioPooling.ReleaseSequenceDefinition(this);
        }
        
        internal void Reset()
        {
            settings.Reset();
            steps.Clear();
            storage.Clear();
        }
    }
}