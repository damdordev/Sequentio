using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening color values.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    [Serializable]
    public abstract class ColorTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Color>
        where TComponent : class
    {
        /// <summary>
        /// The color channels modified by the tween.
        /// </summary>
        public VarioValue<ColorChannelFilter> ChannelFilter
        {
            get => channelFilter;
            set => channelFilter = value;
        }

        /// <summary>
        /// The hue, saturation, and value components modified relative to the captured initial color.
        /// </summary>
        public VarioValue<ColorHSVFilter> HSVFilter
        {
            get => hsvFilter;
            set => hsvFilter = value;
        }

        [SerializeField] private VarioValue<ColorChannelFilter> channelFilter;
        [SerializeField] private VarioValue<ColorHSVFilter> hsvFilter;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            channelFilter = default;
            hsvFilter = default;
        }
    }
}
