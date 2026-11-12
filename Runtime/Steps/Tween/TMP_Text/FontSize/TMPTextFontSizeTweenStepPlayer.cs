using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening font size on a TextMesh Pro component.
    /// </summary>
    public class TMPTextFontSizeTweenStepPlayer : FloatTweenStepPlayer<TMPTextFontSizeTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override float GetValue(TMP_Text component) => component.fontSize;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, float value) => component.fontSize = value;
    }
}
