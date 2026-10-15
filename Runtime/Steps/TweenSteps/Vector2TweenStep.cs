using System;
using Damdor.Vario;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector2TweenStep<TComponent> : TweenStep<TComponent, Vector2> 
        where TComponent : Object
    {
        public VarioValue<Vector2AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }

        private Vector2AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = axisFilter.Evaluate(Storage);
        }

        protected override Vector2 TransformValue(TComponent target, Vector2 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not Vector2TweenStep<TComponent> other) return;
            axisFilter = other.axisFilter;
        }

        [SerializeField] private VarioValue<Vector2AxisFilter> axisFilter;
    }
}