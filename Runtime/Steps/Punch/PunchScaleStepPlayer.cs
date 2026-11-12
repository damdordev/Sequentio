using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for punching transform local scale.
    /// </summary>
    public class PunchScaleStepPlayer : PunchStepPlayer<PunchScaleStepDefinition, Transform>
    {
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform target) => target.localScale;
        /// <inheritdoc />
        protected override void SetValue(Transform target, Vector3 value) => target.localScale = value;
    }
}