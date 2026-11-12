using System;
using Damdor.Vario;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    /// <summary>
    /// Evaluates tween endpoints and easing, applies interpolated values, and restores the captured initial value.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    /// <typeparam name="TValue">The type of value assigned or interpolated.</typeparam>
    public abstract class TweenStepPlayer<TTweenStepDefinition, TComponent, TValue> : StepPlayer<TTweenStepDefinition>
        where TTweenStepDefinition : TweenStepDefinition<TComponent, TValue>
        where TComponent : class
    {
        /// <summary>
        /// The target value captured during initialization.
        /// </summary>
        protected TValue initValue { get; private set; }
        /// <summary>
        /// The target resolved during initialization.
        /// </summary>
        protected TComponent target { get; private set; }
        /// <summary>
        /// The evaluated starting value of the tween.
        /// </summary>
        protected TValue start { get; private set; }
        /// <summary>
        /// The evaluated ending value of the tween.
        /// </summary>
        protected TValue end { get; private set; }
        /// <summary>
        /// The easing curve resolved during initialization.
        /// </summary>
        protected AnimationCurve easing { get; private set; }
        
        /// <inheritdoc />
        protected override void Init(TTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            start = Evaluate(definition.Start, target, storage);
            end = Evaluate(definition.End, target, storage);
            easing = definition.Easing.Evaluate(storage);

            initValue = GetValue(target);
        }

        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);

            SetValue(target, TransformValue(GetValue(target), Lerp(start, end, easing.Evaluate(t))));
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initValue);
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Reads the animated or assigned value from the target.
        /// </summary>
        /// <param name="component">The component whose value is read or written.</param>
        /// <returns>The current value of the target property.</returns>
        protected abstract TValue GetValue(TComponent component);
        /// <summary>
        /// Writes the animated or assigned value to the target.
        /// </summary>
        /// <param name="component">The component whose value is read or written.</param>
        /// <param name="value">The value to apply or convert.</param>
        protected abstract void SetValue(TComponent component, TValue value);
        /// <summary>
        /// Transforms an interpolated value before applying it, preserving unaffected components where appropriate.
        /// </summary>
        /// <param name="currentValue">The current target value, used to preserve unaffected axes or channels.</param>
        /// <param name="value">The value to apply or convert.</param>
        /// <returns>The value to write to the target.</returns>
        protected virtual TValue TransformValue(TValue currentValue, TValue value) => value;
        
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// Interpolates between two values using the registered Vario numeric operations without clamping the factor.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="t">The normalized progress or interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        protected virtual TValue Lerp(TValue a, TValue b, float t)
            => VarioSettings.GetNumericOperations<TValue>().LerpUnclamped(a, b, t);

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// Adds two values using the registered Vario numeric operations.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The sum of the two values.</returns>
        protected virtual TValue Sum(TValue a, TValue b)
            => VarioSettings.GetNumericOperations<TValue>().Sum(a, b);
    }
}