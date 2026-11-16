using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting CanvasGroup.blocksRaycasts.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetCanvasGroupBlocksRaycastsStepPlayer), "CanvasGroup/BlocksRaycasts", "BlocksRaycasts", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetCanvasGroupBlocksRaycastsStep_icon.png")]
    public class SetCanvasGroupBlocksRaycastsStepDefinition : ValueImmediateStepDefinition<CanvasGroup, bool>
    {
    }
}
