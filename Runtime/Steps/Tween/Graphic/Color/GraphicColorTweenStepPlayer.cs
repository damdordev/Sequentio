using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class GraphicColorTweenStepPlayer : ColorTweenStepPlayer<GraphicColorTweenStepDefinition, Graphic>
    {
        protected override Color GetValue(Graphic component) => component.color;
        protected override void SetValue(Graphic component, Color value) => component.color = value;
    }
}
