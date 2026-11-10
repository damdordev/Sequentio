using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class LayoutElementMinHeightTweenStepPlayer : FloatTweenStepPlayer<LayoutElementMinHeightTweenStepDefinition, LayoutElement>
    {
        protected override float GetValue(LayoutElement component) => component.minHeight;
        protected override void SetValue(LayoutElement component, float value) => component.minHeight = value;
    }
}
