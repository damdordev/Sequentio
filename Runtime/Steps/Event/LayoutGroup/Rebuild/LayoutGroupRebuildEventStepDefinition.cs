using System;
using Damdor.Vario;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(LayoutGroupRebuildEventStepPlayer), "LayoutGroup/Rebuild", "Rebuild", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutGroupRebuildEventStepDefinition : EventStepDefinition
    {
        public VarioValue<LayoutGroup> Target
        {
            get => target;
            set => target = value;
        }

        public VarioValue<bool> Immediate
        {
            get => immediate;
            set => immediate = value;
        }

        [SerializeField] private VarioValue<LayoutGroup> target;
        [SerializeField] private VarioValue<bool> immediate;

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);

        public override void Reset()
        {
            base.Reset();
            target = default;
            immediate = default;
        }
    }
}
