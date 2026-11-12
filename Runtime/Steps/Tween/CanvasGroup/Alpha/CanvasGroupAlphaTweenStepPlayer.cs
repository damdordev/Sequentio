using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening canvas group opacity.
    /// </summary>
    public class CanvasGroupAlphaTweenStepPlayer : FloatTweenStepPlayer<CanvasGroupAlphaTweenStepDefinition, CanvasGroup>
    {
        /// <inheritdoc />
        protected override float GetValue(CanvasGroup component) => component.alpha;
        /// <inheritdoc />
        protected override void SetValue(CanvasGroup component, float value) => component.alpha = value;
    }
}
