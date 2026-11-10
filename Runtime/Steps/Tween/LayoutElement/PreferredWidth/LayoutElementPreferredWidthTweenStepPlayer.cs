using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class LayoutElementPreferredWidthTweenStepPlayer : FloatTweenStepPlayer<LayoutElementPreferredWidthTweenStepDefinition, LayoutElement>
    {
        protected override float GetValue(LayoutElement component) => component.preferredWidth;
        protected override void SetValue(LayoutElement component, float value) => component.preferredWidth = value;
    }
}
