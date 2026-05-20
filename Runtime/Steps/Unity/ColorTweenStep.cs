using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class ColorTweenStep<TComponent> : TweenStep<TComponent, Color> 
    {
        public VarioValue<ColorChannelFilter> ChannelFilter
        {
            get => channelFilter;
            set => channelFilter = value;
        }

        private ColorChannelFilter v_ChannelFilter;

        public override void Init()
        {
            base.Init();
            v_ChannelFilter = channelFilter.Evaluate(Storage);
        }

        protected override Color TransformValue(TComponent target, Color value)
            => GetValue(target).Modify(value, v_ChannelFilter);

        internal override void Reset()
        {
            base.Reset();
            channelFilter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not ColorTweenStep<TComponent> other) return;
            channelFilter = other.channelFilter;
        }

        [SerializeField] private VarioValue<ColorChannelFilter> channelFilter;
    }
}