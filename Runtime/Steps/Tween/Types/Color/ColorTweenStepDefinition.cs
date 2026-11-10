using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class ColorTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Color>
        where TComponent : class
    {
        public VarioValue<ColorChannelFilter> ChannelFilter
        {
            get => channelFilter;
            set => channelFilter = value;
        }

        public VarioValue<ColorHSVFilter> HSVFilter
        {
            get => hsvFilter;
            set => hsvFilter = value;
        }

        [SerializeField] private VarioValue<ColorChannelFilter> channelFilter;
        [SerializeField] private VarioValue<ColorHSVFilter> hsvFilter;

        public override void Reset()
        {
            base.Reset();
            channelFilter = default;
            hsvFilter = default;
        }
    }
}
