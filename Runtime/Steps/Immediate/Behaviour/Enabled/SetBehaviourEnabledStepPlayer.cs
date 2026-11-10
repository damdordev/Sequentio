using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetBehaviourEnabledStepPlayer : ValueImmediateStepPlayer<SetBehaviourEnabledStepDefinition, Behaviour, bool>
    {
        protected override bool GetValue(Behaviour component) => component.enabled;
        protected override void SetValue(Behaviour component, bool value) => component.enabled = value;
    }
}
