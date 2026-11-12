using TMPro;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening color on a TextMesh Pro component.
    /// </summary>
    public class TMPTextColorTweenStepPlayer : ColorTweenStepPlayer<TMPTextColorTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override Color GetValue(TMP_Text component) => component.color;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, Color value) => component.color = value;
    }
}
