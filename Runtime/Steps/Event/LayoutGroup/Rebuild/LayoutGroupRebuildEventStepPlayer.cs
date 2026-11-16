using Damdor.Vario;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class LayoutGroupRebuildEventStepPlayer : EventStepPlayer<LayoutGroupRebuildEventStepDefinition>
    {
        private LayoutGroup target;
        private bool immediate;

        protected override void Init(LayoutGroupRebuildEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            immediate = definition.Immediate.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            if (immediate) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)target.transform);
            else LayoutRebuilder.MarkLayoutForRebuild((RectTransform)target.transform);
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
            immediate = default;
        }
    }
}
