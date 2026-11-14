using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Selectable.interactable.
    /// </summary>
    public class SetSelectableInteractableStepPlayer : ValueImmediateStepPlayer<SetSelectableInteractableStepDefinition, Selectable, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(Selectable target) => target.interactable;
        /// <inheritdoc />
        protected override void SetValue(Selectable target, bool value) => target.interactable = value;
    }
}
