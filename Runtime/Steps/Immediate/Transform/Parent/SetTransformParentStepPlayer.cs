using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetTransformParentStepPlayer : ValueImmediateStepPlayer<SetTransformParentStepDefinition, Transform, Transform>
    {
        protected override Transform GetValue(Transform target) => target.parent;
        protected override void SetValue(Transform target, Transform value) => target.parent = value;
    }
}
