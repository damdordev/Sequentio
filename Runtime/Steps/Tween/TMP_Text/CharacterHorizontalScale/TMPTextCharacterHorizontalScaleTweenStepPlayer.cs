using TMPro;

namespace Damdor.Sequentio
{
    public class TMPTextCharacterHorizontalScaleTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterHorizontalScaleTweenStepDefinition, TMP_Text>
    {
        protected override float GetValue(TMP_Text component) => component.characterHorizontalScale;
        protected override void SetValue(TMP_Text component, float value) => component.characterHorizontalScale = value;
    }
}
