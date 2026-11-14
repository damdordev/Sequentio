using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting CanvasGroup.interactable.
    /// </summary>
    public class SetCanvasGroupInteractableStepPlayer : ValueImmediateStepPlayer<SetCanvasGroupInteractableStepDefinition, CanvasGroup, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(CanvasGroup target) => target.interactable;
        /// <inheritdoc />
        protected override void SetValue(CanvasGroup target, bool value) => target.interactable = value;
    }
}
