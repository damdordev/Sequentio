using Damdor.Progressio;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class ProgressBarValueTweenStepPlayer : FloatTweenStepPlayer<ProgressBarValueTweenStepDefinition, ProgressBar>
    {
        private bool immediate;

        protected override void Init(ProgressBarValueTweenStepDefinition definition, VarioStorage storage)
        {
            immediate = definition.Immediate.Evaluate(storage);
            base.Init(definition, storage);
        }

        protected override float GetValue(ProgressBar component) => component.Value;

        protected override void SetValue(ProgressBar component, float value)
        {
            if (immediate) component.SetValueWithoutAnimation(value);
            else component.Value = value;
        }

        public override void Reset()
        {
            base.Reset();
            immediate = default;
        }
    }
}
