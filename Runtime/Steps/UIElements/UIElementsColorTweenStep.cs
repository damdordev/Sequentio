using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    public abstract class UIElementsColorTweenStep<TComponent> : UIElementsTweenStep<TComponent, Color>
        where TComponent : VisualElement
    {
        public VarioValue<ColorChannelFilter> Filter
        {
            get => filter;
            set => filter = value;
        }

        private ColorChannelFilter v_filter;

        public override void Init()
        {
            base.Init();
            v_filter = filter.Evaluate(Storage);
        }

        protected override Color TransformValue(TComponent target, Color value)
            => GetValue(target).Modify(value, v_filter);

        internal override void Reset()
        {
            base.Reset();
            filter = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not UIElementsColorTweenStep<TComponent> other) return;
            filter = other.filter;
        }

        [SerializeField] private VarioValue<ColorChannelFilter> filter;
    }
}