using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Selectable.interactable.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetSelectableInteractableStepPlayer), "Selectable/Interactable", "Interactable", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetSelectableInteractableStep_icon.png")]
    public class SetSelectableInteractableStepDefinition : ValueImmediateStepDefinition<Selectable, bool>
    {
    }
}
