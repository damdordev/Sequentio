using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting CanvasGroup.blocksRaycasts.
    /// </summary>
    public class SetCanvasGroupBlocksRaycastsStepPlayer : ValueImmediateStepPlayer<SetCanvasGroupBlocksRaycastsStepDefinition, CanvasGroup, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(CanvasGroup target) => target.blocksRaycasts;
        /// <inheritdoc />
        protected override void SetValue(CanvasGroup target, bool value) => target.blocksRaycasts = value;
    }
}
