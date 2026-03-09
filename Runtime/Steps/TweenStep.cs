using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public struct TweenPoint<TComponent, TValue>
    {
        public TweenPointSource ValueSource
        {
            get => valueSource;
            set => valueSource = value;
        }

        public VarioValue<TComponent> OtherTarget
        {
            get => otherTarget;
            set => otherTarget = value;
        }
        
        public VarioValue<TValue> Value
        {
            get => value;
            set => this.value = value;
        }
        
        [SerializeField] private TweenPointSource valueSource;
        [SerializeField] private VarioValue<TComponent> otherTarget;
        [SerializeField] private VarioValue<TValue> value;
            
        public static implicit operator TweenPoint<TComponent, TValue>(TValue value) => new() { ValueSource = TweenPointSource.Value, Value = value };

    }
    
    public abstract class TweenStep<TComponent, TValue> : SequentioStep
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
        
        public override float CalculateDuration() => (float) Storage.Evaluate(duration).TotalSeconds;
        
        public override void Init()
        {
            base.Init();
            v_target = Storage.Evaluate(target);
            v_start = Evaluate(start);
            v_end = Evaluate(end);
            v_easing = Storage.Evaluate(easing);
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
        protected abstract TValue Lerp(TValue a, TValue b, float t);
        protected abstract TValue Sum(TValue a, TValue b);

        private TValue Evaluate(TweenPoint<TComponent, TValue> point) => point.ValueSource switch
        {
            TweenPointSource.Value => Storage.Evaluate(point.Value),
            TweenPointSource.Target => Sum(GetValue(v_target), Storage.Evaluate(point.Value)),
            TweenPointSource.OtherTarget => Sum(GetValue(Storage.Evaluate(point.OtherTarget)),
                Storage.Evaluate(point.Value)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}