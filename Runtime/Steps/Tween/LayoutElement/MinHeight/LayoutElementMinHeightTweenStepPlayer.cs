using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening min height on a LayoutElement.
    /// </summary>
    public class LayoutElementMinHeightTweenStepPlayer : FloatTweenStepPlayer<LayoutElementMinHeightTweenStepDefinition, LayoutElement>
    {
        /// <inheritdoc />
        protected override float GetValue(LayoutElement component) => component.minHeight;
        /// <inheritdoc />
        protected override void SetValue(LayoutElement component, float value) => component.minHeight = value;
    }
}
