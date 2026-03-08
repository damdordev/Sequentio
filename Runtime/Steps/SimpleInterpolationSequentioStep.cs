using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class SimpleInterpolationSequentioStep<TComponent, TValue> : SequentioStep
    {
        #region Types
        
        public enum ValueSource
        {
            Value,
            Target,
            OtherTarget
        }
        
        [Serializable]
        public struct Point<TComponent, TValue>
        {
            public ValueSource ValueSource
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
        
            [SerializeField] private ValueSource valueSource;
            [SerializeField] private VarioValue<TComponent> otherTarget;
            [SerializeField] private VarioValue<TValue> value;
        }
        
        #endregion

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

        public Point<TComponent, TValue> Start
        {
            get => start;
            set => start = value;
        }

        public Point<TComponent, TValue> End
        {
            get => end;
            set => end = value;
        }
        
        [SerializeField] private VarioValue<TComponent> target;
        [SerializeField] private VarioValue<TimeSpan, SerializableTimeSpan> duration;
        [SerializeField] private VarioValue<AnimationCurve> easing = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private Point<TComponent, TValue> start;
        [SerializeField] private Point<TComponent, TValue> end;

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

        private TValue Evaluate(Point<TComponent, TValue> point) => point.ValueSource switch
        {
            ValueSource.Value => Storage.Evaluate(point.Value),
            ValueSource.Target => Sum(GetValue(v_target), Storage.Evaluate(point.Value)),
            ValueSource.OtherTarget => Sum(GetValue(Storage.Evaluate(point.OtherTarget)),
                Storage.Evaluate(point.Value)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}