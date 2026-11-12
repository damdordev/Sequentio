using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening color values.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class ColorTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Color>
        where TTweenStepDefinition : ColorTweenStepDefinition<TComponent>
        where TComponent : class
    {
        private ColorChannelFilter channelFilter;
        private ColorHSVFilter hsvFilter;

        /// <inheritdoc />
        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            channelFilter = definition.ChannelFilter.Evaluate(storage);
            hsvFilter = definition.HSVFilter.Evaluate(storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            channelFilter = default;
            hsvFilter = default;
        }

        /// <inheritdoc />
        protected override Color TransformValue(Color currentValue, Color value)
        {
            return currentValue.Modify(initValue.Modify(value, hsvFilter), channelFilter);
        }
    }
}
