using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening four-dimensional vector values.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class Vector4TweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Vector4>
        where TTweenStepDefinition : Vector4TweenStepDefinition<TComponent>
        where TComponent : class
    {
        private Vector4AxisFilter axisFilter;

        // ReSharper disable Unity.PerformanceAnalysis
        /// <inheritdoc />
        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            axisFilter = definition.AxisFilter.Evaluate(storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }

        /// <inheritdoc />
        protected override Vector4 TransformValue(Vector4 currentValue, Vector4 value) => currentValue.Modify(value, axisFilter);
    }
}
