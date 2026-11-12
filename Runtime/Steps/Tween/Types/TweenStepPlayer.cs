using System;
using Damdor.Vario;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public abstract class TweenStepPlayer<TTweenStepDefinition, TComponent, TValue> : StepPlayer<TTweenStepDefinition>
        where TTweenStepDefinition : TweenStepDefinition<TComponent, TValue>
        where TComponent : class
    {
        protected TValue initValue { get; private set; }
        protected TComponent target { get; private set; }
        protected TValue start { get; private set; }
        protected TValue end { get; private set; }
        protected AnimationCurve easing { get; private set; }
        
        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            start = Evaluate(definition.Start, target, storage);
            end = Evaluate(definition.End, target, storage);
            easing = definition.Easing.Evaluate(storage);

            initValue = GetValue(target);
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);

            SetValue(target, TransformValue(GetValue(target), Lerp(start, end, easing.Evaluate(t))));
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initValue);
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
            start = default;
            end = default;
            easing = null;
            initValue = default;
        }

        private TValue Evaluate(TweenEndpoint<TComponent, TValue> endpoint, TComponent component, VarioStorage storage)
            => endpoint.ValueSource switch
        {
            TweenPointSource.ExplicitValue => endpoint.Value.Evaluate(storage),
            TweenPointSource.CurrentValue => Sum(GetValue(component), endpoint.Value.Evaluate(storage)),
            TweenPointSource.ReferenceTarget => Sum(GetValue(endpoint.OtherTarget.Evaluate(storage)), endpoint.Value.Evaluate(storage)),
            _ => throw new ArgumentOutOfRangeException()
        };

        protected abstract TValue GetValue(TComponent component);
        protected abstract void SetValue(TComponent component, TValue value);
        protected virtual TValue TransformValue(TValue currentValue, TValue value) => value;
        
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual TValue Lerp(TValue a, TValue b, float t)
            => VarioSettings.GetNumericOperations<TValue>().LerpUnclamped(a, b, t);

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual TValue Sum(TValue a, TValue b)
            => VarioSettings.GetNumericOperations<TValue>().Sum(a, b);
    }
}