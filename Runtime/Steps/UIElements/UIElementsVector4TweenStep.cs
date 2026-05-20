using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    public abstract class UIElementsVector4TweenStep<TComponent> : UIElementsTweenStep<TComponent, Vector4>
        where TComponent : VisualElement
    {
        public VarioValue<Vector4AxisFilter> Filter
        {
            get => filter;
            set => filter = value;
        }

        private Vector4AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = filter.Evaluate(Storage);
        }

        protected override Vector4 TransformValue(TComponent target, Vector4 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            filter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not UIElementsVector4TweenStep<TComponent> other) return;
            filter = other.filter;
        }

        [SerializeField] private VarioValue<Vector4AxisFilter> filter;
    }
}