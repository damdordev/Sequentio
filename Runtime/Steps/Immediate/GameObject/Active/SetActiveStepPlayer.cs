using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the active state of a game object.
    /// </summary>
    public class SetActiveStepPlayer : ValueImmediateStepPlayer<SetActiveStepDefinition, GameObject, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(GameObject target) => target.activeSelf;
        /// <inheritdoc />
        protected override void SetValue(GameObject target, bool value) => target.SetActive(value);
    }
}