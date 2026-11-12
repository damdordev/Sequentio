using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class Vector2TweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Vector2>
        where TTweenStepDefinition : Vector2TweenStepDefinition<TComponent>
        where TComponent : class
    {
        private Vector2AxisFilter axisFilter;

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

        protected override Vector2 TransformValue(Vector2 currentValue, Vector2 value) => currentValue.Modify(value, axisFilter);
    }
}
