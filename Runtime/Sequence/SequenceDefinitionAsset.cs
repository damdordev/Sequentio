using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [CreateAssetMenu(fileName = "sequence", menuName = "Damdor/Sequntio/Sequence asset")]
    public class SequenceDefinitionAsset : ScriptableObject, ISequenceDefinition, IVarioStorageSource
    {
        public SequenceSettings Settings => settings;
        public IReadOnlyList<SequenceStep> Steps => steps;
        public VarioStorage Storage => storage;
        
        [SerializeField] private VarioStorage storage = new();
        [SerializeReference] private List<SequenceStep> steps = new();
        [SerializeField] private SequenceSettings settings = new();
        
        public void AddStep(SequenceStep step) => steps.Add(step);
    }
}