using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class Vector4TweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Vector4>
        where TTweenStepDefinition : Vector4TweenStepDefinition<TComponent>
        where TComponent : class
    {
        private Vector4AxisFilter axisFilter;

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            axisFilter = definition.AxisFilter.Evaluate(storage);
        }

        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }

        protected override Vector4 TransformValue(Vector4 currentValue, Vector4 value) => currentValue.Modify(value, axisFilter);
    }
}
