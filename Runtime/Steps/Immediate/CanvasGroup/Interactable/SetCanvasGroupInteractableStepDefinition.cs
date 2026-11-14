using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting CanvasGroup.interactable.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetCanvasGroupInteractableStepPlayer), "CanvasGroup/Interactable", "Interactable", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetCanvasGroupInteractableStepDefinition : ValueImmediateStepDefinition<CanvasGroup, bool>
    {
    }
}
