using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class Vector3TweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Vector3>
        where TTweenStepDefinition : Vector3TweenStepDefinition<TComponent>
        where TComponent : class
    {
        private Vector3AxisFilter axisFilter;

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

        protected override Vector3 TransformValue(Vector3 currentValue, Vector3 value) => currentValue.Modify(value, axisFilter);
    }
}