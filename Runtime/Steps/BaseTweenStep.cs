using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class BaseTweenStep<TComponent, TComponentPointer, TValue> : SequenceStep
        where TComponent : class
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

        public TweenEndpoint<TComponent, TValue> Start
        {
            get => start;
            set => start = value;
        }

        public TweenEndpoint<TComponent, TValue> End
        {
            get => end;
            set => end = value;
        }
        
        [SerializeField] private TComponentPointer target;
        [SerializeField] private VarioValue<float> duration;
        [SerializeField] private VarioValue<AnimationCurve> easing = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private TweenEndpoint<TComponent, TValue> start;
        [SerializeField] private TweenEndpoint<TComponent, TValue> end;

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

        protected TValue Evaluate(TweenEndpoint<TComponent, TValue> endpoint) => endpoint.ValueSource switch
        {
            TweenPointSource.ExplicitValue => endpoint.Value.Evaluate(Storage),
            TweenPointSource.CurrentValue => Sum(GetValue(GetComponent(target)), endpoint.Value.Evaluate(Storage)),
            TweenPointSource.ReferenceTarget => Sum(
                GetValue(endpoint.OtherTarget.Evaluate(Storage) ?? GetComponent(target)),
                endpoint.Value.Evaluate(Storage)
            ),
            _ => throw new ArgumentOutOfRangeException()
        };

        protected virtual TValue Lerp(TValue a, TValue b, float t)
            => VarioSettings.GetNumericOperations<TValue>().LerpUnclamped(a, b, t);

        protected virtual TValue Sum(TValue a, TValue b)
            => VarioSettings.GetNumericOperations<TValue>().Sum(a, b);
        
    }
}