using Damdor.Vario;

namespace Damdor.Sequentio
{
    public abstract class ValueImmediateStepPlayer<TStepDefinition, TComponent, TValue> : StepPlayer<TStepDefinition>
        where TStepDefinition : ValueImmediateStepDefinition<TComponent, TValue>
        where TComponent : class
    {
        private TComponent target;
        private TValue initialValue;
        private TValue targetValue;

        protected override void Init(TStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            initialValue = GetValue(target);
            targetValue = definition.Value.Evaluate(storage);
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            SetValue(target, t < 0.5f ? initialValue : targetValue);
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
            initialValue = default;
            targetValue = default;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initialValue);
        }

        protected abstract TValue GetValue(TComponent target);
        protected abstract void SetValue(TComponent target, TValue value);
    }
}