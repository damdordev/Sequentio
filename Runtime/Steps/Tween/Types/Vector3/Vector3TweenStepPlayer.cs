using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening three-dimensional vector values.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class Vector3TweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Vector3>
        where TTweenStepDefinition : Vector3TweenStepDefinition<TComponent>
        where TComponent : class
    {
        private Vector3AxisFilter axisFilter;

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
        protected override Vector3 TransformValue(Vector3 currentValue, Vector3 value) => currentValue.Modify(value, axisFilter);
    }
}