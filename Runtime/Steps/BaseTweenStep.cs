using System;
using Damdor.Numerio;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class BaseTweenStep<TComponent, TComponentPointer, TValue> : SequenceStep
    {
        public TComponentPointer Target
        {
            get => target;
            set => target = value;
        }

        public VarioValue<float> Duration
        {
            get => duration;
            set => duration = value;
        }

        public VarioValue<AnimationCurve> Easing
        {
            get => easing;
            set => easing = value;
        }

        public TweenPoint<TComponent, TValue> Start
        {
            get => start;
            set => start = value;
        }

        public TweenPoint<TComponent, TValue> End
        {
            get => end;
            set => end = value;
        }
        
        [SerializeField] private TComponentPointer target;
        [SerializeField] private VarioValue<float> duration;
        [SerializeField] private VarioValue<AnimationCurve> easing = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private TweenPoint<TComponent, TValue> start;
        [SerializeField] private TweenPoint<TComponent, TValue> end;

        private TValue initialValue;
        private TComponent v_target;
        private TValue v_start;
        private TValue v_end;
        private AnimationCurve v_easing;

        public override float CalculateDuration() => duration.Evaluate(Storage);
        
        public override void Init()
        {
            base.Init();
            v_target = GetComponent(target);
            v_start = Evaluate(start);
            v_end = Evaluate(end);
            v_easing = easing.Evaluate(Storage);
            initialValue = GetValue(v_target);
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(v_target, TransformValue(v_target, initialValue));
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            SetValue(v_target, TransformValue(v_target, Lerp(v_start, v_end, v_easing.Evaluate(t))));
        }

        internal override void Reset()
        {
            base.Reset();
            target = default;
            duration = 1f;
            easing = AnimationCurve.Linear(0, 0, 1, 1);
            start = default;
            end = default;
            v_target = default;
            v_start = default;
            v_end = default;
            v_easing = null;
            initialValue = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not BaseTweenStep<TComponent, TComponentPointer, TValue> other) return;
            target = other.target;
            duration = other.duration;
            easing = other.easing;
            start = other.start;
            end = other.end;
        }

        protected abstract TValue GetValue(TComponent target);
        protected abstract void SetValue(TComponent target, TValue value);
        protected abstract TComponent GetComponent(TComponentPointer pointer);
        protected virtual TValue TransformValue(TComponent target, TValue value) => value;

        private TValue Evaluate(TweenPoint<TComponent, TValue> point) => point.ValueSource switch
        {
            TweenPointSource.Value => point.Value.Evaluate(Storage),
            TweenPointSource.Target => Sum(GetValue(v_target), point.Value.Evaluate(Storage)),
            TweenPointSource.OtherTarget => Sum(
                GetValue(point.OtherTarget.Evaluate(Storage)),
                point.Value.Evaluate(Storage)
            ),
            _ => throw new ArgumentOutOfRangeException()
        };

        protected virtual TValue Lerp(TValue a, TValue b, float t)
            => NumerioSettings.Get<TValue>().LerpUnclamped(a, b, t);

        protected virtual TValue Sum(TValue a, TValue b)
            => NumerioSettings.Get<TValue>().Sum(a, b);
        
    }
}