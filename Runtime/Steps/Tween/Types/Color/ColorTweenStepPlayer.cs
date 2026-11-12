using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class ColorTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, Color>
        where TTweenStepDefinition : ColorTweenStepDefinition<TComponent>
        where TComponent : class
    {
        private ColorChannelFilter channelFilter;
        private ColorHSVFilter hsvFilter;

        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            channelFilter = definition.ChannelFilter.Evaluate(storage);
            hsvFilter = definition.HSVFilter.Evaluate(storage);
        }

        public override void Reset()
        {
            base.Reset();
            channelFilter = default;
            hsvFilter = default;
        }

        protected override Color TransformValue(Color currentValue, Color value)
        {
            return currentValue.Modify(initValue.Modify(value, hsvFilter), channelFilter);
        }
    }
}
