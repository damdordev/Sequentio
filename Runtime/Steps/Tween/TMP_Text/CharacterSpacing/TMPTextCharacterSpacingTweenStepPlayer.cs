using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening character spacing on a TextMesh Pro component.
    /// </summary>
    public class TMPTextCharacterSpacingTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterSpacingTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override float GetValue(TMP_Text component) => component.characterSpacing;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, float value) => component.characterSpacing = value;
    }
}
