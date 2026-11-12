using Damdor.Vario;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Captures a target value and switches between the initial and configured values at half progress.
    /// </summary>
    /// <typeparam name="TStepDefinition">The step definition type handled by the player or compiler.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    /// <typeparam name="TValue">The type of value assigned or interpolated.</typeparam>
    public abstract class ValueImmediateStepPlayer<TStepDefinition, TComponent, TValue> : StepPlayer<TStepDefinition>
        where TStepDefinition : ValueImmediateStepDefinition<TComponent, TValue>
        where TComponent : class
    {
        private TComponent target;
        private TValue initialValue;
        private TValue targetValue;

        /// <inheritdoc />
        protected override void Init(TStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            initialValue = GetValue(target);
            targetValue = definition.Value.Evaluate(storage);
        }

        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            SetValue(target, t < 0.5f ? initialValue : targetValue);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            target = null;
            initialValue = default;
            targetValue = default;
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initialValue);
        }

        /// <summary>
        /// Reads the animated or assigned value from the target.
        /// </summary>
        /// <param name="target">The target whose value is read or written.</param>
        /// <returns>The current value of the target property.</returns>
        protected abstract TValue GetValue(TComponent target);
        /// <summary>
        /// Writes the animated or assigned value to the target.
        /// </summary>
        /// <param name="target">The target whose value is read or written.</param>
        /// <param name="value">The value to apply or convert.</param>
        protected abstract void SetValue(TComponent target, TValue value);
    }
}