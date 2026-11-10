using Damdor.Traduio;

namespace Damdor.Sequentio
{
    public class BaseNumberTranslationTargetValueTweenStepPlayer : FloatTweenStepPlayer<BaseNumberTranslationTargetValueTweenStepDefinition, BaseNumberTranslationTarget>
    {
        protected override float GetValue(BaseNumberTranslationTarget component) => component.Value;
        protected override void SetValue(BaseNumberTranslationTarget component, float value) => component.Value = value;
    }
}
