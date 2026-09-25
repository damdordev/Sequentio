using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector4TweenStep<TComponent> : TweenStep<TComponent, Vector4> 
    {
        public VarioValue<Vector4AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }

        private Vector4AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = axisFilter.Evaluate(Storage);
        }

        protected override Vector4 TransformValue(TComponent target, Vector4 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not Vector4TweenStep<TComponent> other) return;
            axisFilter = other.axisFilter;
        }

        [SerializeField] private VarioValue<Vector4AxisFilter> axisFilter;
    }
}