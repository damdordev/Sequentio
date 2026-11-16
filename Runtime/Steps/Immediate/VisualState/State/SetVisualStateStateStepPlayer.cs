using Damdor.Statio;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class SetVisualStateStateStepPlayer : ValueImmediateStepPlayer<SetVisualStateStateStepDefinition, VisualState, string>
    {
        private bool immediate;

        protected override void Init(SetVisualStateStateStepDefinition definition, VarioStorage storage)
        {
            immediate = definition.Immediate.Evaluate(storage);
            base.Init(definition, storage);
        }

        protected override string GetValue(VisualState component) => component.CurrentState;

        protected override void SetValue(VisualState component, string value)
        {
            if (immediate) component.ChangeStateImmediately(value);
            else component.ChangeState(value);
        }

        public override void Reset()
        {
            base.Reset();
            immediate = default;
        }
    }
}
