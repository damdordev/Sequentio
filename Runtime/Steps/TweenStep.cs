using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class TweenStep<TComponent, TValue> : SequenceStep
    {
        public VarioValue<TComponent> Target
        {
            get => target;
            set => target = value;
        }

        public VarioValue<TimeSpan, SerializableTimeSpan> Duration
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
        
        [SerializeField] private VarioValue<TComponent> target;
        [SerializeField] private VarioValue<TimeSpan, SerializableTimeSpan> duration;
        [SerializeField] private VarioValue<AnimationCurve> easing = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private TweenPoint<TComponent, TValue> start;
        [SerializeField] private TweenPoint<TComponent, TValue> end;

        private TValue initialValue;
        private TComponent v_target;
        private TValue v_start;
        private TValue v_end;
        private AnimationCurve v_easing;

        public override float CalculateDuration() => (float)duration.Evaluate(Storage).TotalSeconds;
        
        public override void Init()
        {
            base.Init();
            v_target = target.Evaluate(Storage);
            v_start = Evaluate(start);
            v_end = Evaluate(end);
            v_easing = easing.Evaluate(Storage);
            initialValue = GetValue(v_target);
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(v_target, initialValue);
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            SetValue(v_target, Lerp(v_start, v_end, v_easing.Evaluate(t)));
        }

        protected abstract TValue GetValue(TComponent target);
        protected abstract void SetValue(TComponent target, TValue value);

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
            => NumericOperations.Get<TValue>().LerpUnclamped(a, b, t);

        protected virtual TValue Sum(TValue a, TValue b)
            => NumericOperations.Get<TValue>().Sum(a, b);
        
    }
}