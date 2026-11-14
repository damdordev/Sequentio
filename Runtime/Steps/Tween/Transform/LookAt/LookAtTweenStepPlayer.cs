using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Interpolates from the captured world rotation to the captured look-at orientation.
    /// </summary>
    public class LookAtTweenStepPlayer : StepPlayer<LookAtTweenStepDefinition>
    {
        private Transform target;
        private Quaternion initialRotation;
        private Quaternion endRotation;
        private AnimationCurve easing;

        /// <inheritdoc />
        protected override void Init(LookAtTweenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            initialRotation = target.rotation;
            var lookAtTarget = definition.LookAtTarget.Evaluate(storage);
            var position = lookAtTarget != null ? lookAtTarget.position : definition.Position.Evaluate(storage);
            var direction = position - target.position;
            var up = definition.Up.Evaluate(storage);
            if (up.sqrMagnitude <= Mathf.Epsilon) up = Vector3.up;
            endRotation = direction.sqrMagnitude <= Mathf.Epsilon
                ? initialRotation
                : Quaternion.LookRotation(direction, up);
            easing = definition.Easing.Evaluate(storage);
        }

        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            target.rotation = Quaternion.SlerpUnclamped(initialRotation, endRotation, easing.Evaluate(t));
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            target.rotation = initialRotation;
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            target = null;
            initialRotation = default;
            endRotation = default;
            easing = null;
        }
    }
}
