using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class LayoutElementMinWidthTweenStepPlayer : FloatTweenStepPlayer<LayoutElementMinWidthTweenStepDefinition, LayoutElement>
    {
        protected override float GetValue(LayoutElement component) => component.minWidth;
        protected override void SetValue(LayoutElement component, float value) => component.minWidth = value;
    }
}
