using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening preferred height on a LayoutElement.
    /// </summary>
    public class LayoutElementPreferredHeightTweenStepPlayer : FloatTweenStepPlayer<LayoutElementPreferredHeightTweenStepDefinition, LayoutElement>
    {
        /// <inheritdoc />
        protected override float GetValue(LayoutElement component) => component.preferredHeight;
        /// <inheritdoc />
        protected override void SetValue(LayoutElement component, float value) => component.preferredHeight = value;
    }
}
