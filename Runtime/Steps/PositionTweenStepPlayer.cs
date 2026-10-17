using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class PositionTweenStepPlayer : Vector3TweenStepPlayer<PositionTweenStepDefinition, Transform>
    {
        private bool local;

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void Init(PositionTweenStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        public override void Reset()
        {
            base.Reset();
            local = true;
        }
        
        protected override Vector3 GetValue(Transform component) => local ? component.localPosition : component.position;

        protected override void SetValue(Transform component, Vector3 value)
        {
            if (local) component.localPosition = value;
            else component.position = value;
        }
    }
}