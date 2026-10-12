using System;
using Damdor.Vario;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector3TweenStep<TComponent> : TweenStep<TComponent, Vector3> 
        where TComponent : Object
    {
        public VarioValue<Vector3AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }

        private Vector3AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = axisFilter.Evaluate(Storage);
        }

        protected override Vector3 TransformValue(TComponent target, Vector3 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not Vector3TweenStep<TComponent> other) return;
            axisFilter = other.axisFilter;
        }

        [SerializeField] private VarioValue<Vector3AxisFilter> axisFilter;
    }
}