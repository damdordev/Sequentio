using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening preferred width on a LayoutElement.
    /// </summary>
    public class LayoutElementPreferredWidthTweenStepPlayer : FloatTweenStepPlayer<LayoutElementPreferredWidthTweenStepDefinition, LayoutElement>
    {
        /// <inheritdoc />
        protected override float GetValue(LayoutElement component) => component.preferredWidth;
        /// <inheritdoc />
        protected override void SetValue(LayoutElement component, float value) => component.preferredWidth = value;
    }
}
