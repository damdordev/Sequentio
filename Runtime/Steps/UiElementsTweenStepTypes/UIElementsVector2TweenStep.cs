using Damdor.Vario;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    public abstract class UIElementsVector2TweenStep<TComponent> : UIElementsTweenStep<TComponent, Vector2>
        where TComponent : VisualElement
    {
        public VarioValue<Vector2AxisFilter> Filter
        {
            get => filter;
            set => filter = value;
        }

        private Vector2AxisFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = filter.Evaluate(Storage);
        }

        protected override Vector2 TransformValue(TComponent target, Vector2 value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            filter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not UIElementsVector2TweenStep<TComponent> other) return;
            filter = other.filter;
        }

        [SerializeField] private VarioValue<Vector2AxisFilter> filter;
    }
}