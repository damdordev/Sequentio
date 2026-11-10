using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class LayoutElementPreferredHeightTweenStepPlayer : FloatTweenStepPlayer<LayoutElementPreferredHeightTweenStepDefinition, LayoutElement>
    {
        protected override float GetValue(LayoutElement component) => component.preferredHeight;
        protected override void SetValue(LayoutElement component, float value) => component.preferredHeight = value;
    }
}
