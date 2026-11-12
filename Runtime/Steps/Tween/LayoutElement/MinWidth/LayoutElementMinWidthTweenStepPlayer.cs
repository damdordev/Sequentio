using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening min width on a LayoutElement.
    /// </summary>
    public class LayoutElementMinWidthTweenStepPlayer : FloatTweenStepPlayer<LayoutElementMinWidthTweenStepDefinition, LayoutElement>
    {
        /// <inheritdoc />
        protected override float GetValue(LayoutElement component) => component.minWidth;
        /// <inheritdoc />
        protected override void SetValue(LayoutElement component, float value) => component.minWidth = value;
    }
}
