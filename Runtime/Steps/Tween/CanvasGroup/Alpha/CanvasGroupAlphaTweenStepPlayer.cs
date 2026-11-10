using UnityEngine;

namespace Damdor.Sequentio
{
    public class CanvasGroupAlphaTweenStepPlayer : FloatTweenStepPlayer<CanvasGroupAlphaTweenStepDefinition, CanvasGroup>
    {
        protected override float GetValue(CanvasGroup component) => component.alpha;
        protected override void SetValue(CanvasGroup component, float value) => component.alpha = value;
    }
}
