using UnityEngine;

namespace Damdor.Sequentio
{
    public class PunchScaleStepPlayer : PunchStepPlayer<PunchScaleStepDefinition, Transform>
    {
        protected override Vector3 GetValue(Transform target) => target.localScale;
        protected override void SetValue(Transform target, Vector3 value) => target.localScale = value;
    }
}