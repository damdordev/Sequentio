using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening alpha on a TextMesh Pro component.
    /// </summary>
    public class TMPTextAlphaTweenStepPlayer : FloatTweenStepPlayer<TMPTextAlphaTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override float GetValue(TMP_Text component) => component.alpha;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, float value) => component.alpha = value;
    }
}
