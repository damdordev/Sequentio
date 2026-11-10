using TMPro;

namespace Damdor.Sequentio
{
    public class TMPTextCharacterWidthAdjustmentTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterWidthAdjustmentTweenStepDefinition, TMP_Text>
    {
        protected override float GetValue(TMP_Text component) => component.characterWidthAdjustment;
        protected override void SetValue(TMP_Text component, float value) => component.characterWidthAdjustment = value;
    }
}
