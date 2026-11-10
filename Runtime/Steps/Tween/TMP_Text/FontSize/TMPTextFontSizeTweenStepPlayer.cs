using TMPro;

namespace Damdor.Sequentio
{
    public class TMPTextFontSizeTweenStepPlayer : FloatTweenStepPlayer<TMPTextFontSizeTweenStepDefinition, TMP_Text>
    {
        protected override float GetValue(TMP_Text component) => component.fontSize;
        protected override void SetValue(TMP_Text component, float value) => component.fontSize = value;
    }
}
