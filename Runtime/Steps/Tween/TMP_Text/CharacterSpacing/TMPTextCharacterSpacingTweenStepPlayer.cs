using TMPro;

namespace Damdor.Sequentio
{
    public class TMPTextCharacterSpacingTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterSpacingTweenStepDefinition, TMP_Text>
    {
        protected override float GetValue(TMP_Text component) => component.characterSpacing;
        protected override void SetValue(TMP_Text component, float value) => component.characterSpacing = value;
    }
}
