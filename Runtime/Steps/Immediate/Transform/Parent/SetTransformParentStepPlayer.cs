using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the parent of a transform.
    /// </summary>
    public class SetTransformParentStepPlayer : ValueImmediateStepPlayer<SetTransformParentStepDefinition, Transform, Transform>
    {
        /// <inheritdoc />
        protected override Transform GetValue(Transform target) => target.parent;
        /// <inheritdoc />
        protected override void SetValue(Transform target, Transform value) => target.parent = value;
    }
}
