using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class Vector3TweenStep<TComponent> : TweenStep<TComponent, Vector3> 
    {
        public VarioValue<Vector3AxisFilter> Filter
        {
            get => filter;
            set => filter = value;
        }

        private Vector3AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = filter.Evaluate(Storage);
        }

        protected override Vector3 TransformValue(TComponent target, Vector3 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            filter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not Vector3TweenStep<TComponent> other) return;
            filter = other.filter;
        }

        [SerializeField] private VarioValue<Vector3AxisFilter> filter;
    }
}