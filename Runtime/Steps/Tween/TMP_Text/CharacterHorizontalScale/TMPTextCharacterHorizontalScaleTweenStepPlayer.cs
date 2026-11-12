using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening character horizontal scale on a TextMesh Pro component.
    /// </summary>
    public class TMPTextCharacterHorizontalScaleTweenStepPlayer : FloatTweenStepPlayer<TMPTextCharacterHorizontalScaleTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override float GetValue(TMP_Text component) => component.characterHorizontalScale;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, float value) => component.characterHorizontalScale = value;
    }
}
