using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetActiveStepPlayer : ValueImmediateStepPlayer<SetActiveStepDefinition, GameObject, bool>
    {
        protected override bool GetValue(GameObject target) => target.activeSelf;
        protected override void SetValue(GameObject target, bool value) => target.SetActive(value);
    }
}