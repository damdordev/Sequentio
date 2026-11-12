using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening character width adjustment on a TextMesh Pro component.
    /// </summary>
    public class TMPTextCharacterWidthAdjustmentTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterWidthAdjustmentTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override float GetValue(TMP_Text component) => component.characterWidthAdjustment;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, float value) => component.characterWidthAdjustment = value;
    }
}
