using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening a UI graphic color.
    /// </summary>
    public class GraphicColorTweenStepPlayer : ColorTweenStepPlayer<GraphicColorTweenStepDefinition, Graphic>
    {
        /// <inheritdoc />
        protected override Color GetValue(Graphic component) => component.color;
        /// <inheritdoc />
        protected override void SetValue(Graphic component, Color value) => component.color = value;
    }
}
