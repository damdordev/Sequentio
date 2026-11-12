using Damdor.Traduio;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening the numeric value of a translation target.
    /// </summary>
    public class BaseNumberTranslationTargetValueTweenStepPlayer : FloatTweenStepPlayer<BaseNumberTranslationTargetValueTweenStepDefinition, BaseNumberTranslationTarget>
    {
        /// <inheritdoc />
        protected override float GetValue(BaseNumberTranslationTarget component) => component.Value;
        /// <inheritdoc />
        protected override void SetValue(BaseNumberTranslationTarget component, float value) => component.Value = value;
    }
}
