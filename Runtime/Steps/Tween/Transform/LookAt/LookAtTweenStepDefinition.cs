using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Tweens world rotation toward a position captured during initialization.
    /// When LookAtTarget is assigned, its world position takes precedence over Position.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LookAtTweenStepPlayer), "Transform/LookAt", "Look at", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LookAtTweenStepDefinition : StepDefinition
    {
        public VarioValue<Transform> Target
        {
            get => target;
            set => target = value;
        }

        /// <summary>
        /// An optional transform whose world position is captured when the step initializes.
        /// </summary>
        public VarioValue<Transform> LookAtTarget
        {
            get => lookAtTarget;
            set => lookAtTarget = value;
        }

        /// <summary>
        /// The world position to face when LookAtTarget is unassigned.
        /// </summary>
        public VarioValue<Vector3> Position
        {
            get => position;
            set => position = value;
        }

        public VarioValue<Vector3> Up
        {
            get => up;
            set => up = value;
        }

        public VarioValue<float> Duration
        {
            get => duration;
            set => duration = value;
        }

        public VarioValue<AnimationCurve> Easing
        {
            get => easing;
            set => easing = value;
        }

        [SerializeField] private VarioValue<Transform> target;
        [SerializeField] private VarioValue<Transform> lookAtTarget;
        [SerializeField] private VarioValue<Vector3> position;
        [SerializeField] private VarioValue<Vector3> up;
        [SerializeField] private VarioValue<float> duration;
        [SerializeField] private VarioValue<AnimationCurve> easing;

        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        /// <inheritdoc />
        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            target = null;
            lookAtTarget = null;
            position = Vector3.zero;
            up = Vector3.up;
            duration = 1f;
            easing = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }
    }
}
