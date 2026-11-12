using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the enabled state of a behaviour.
    /// </summary>
    public class SetBehaviourEnabledStepPlayer : ValueImmediateStepPlayer<SetBehaviourEnabledStepDefinition, Behaviour, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(Behaviour component) => component.enabled;
        /// <inheritdoc />
        protected override void SetValue(Behaviour component, bool value) => component.enabled = value;
    }
}
