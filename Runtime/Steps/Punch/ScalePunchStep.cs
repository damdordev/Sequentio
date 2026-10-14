using System;
using UnityEngine;

namespace Damdor.Sequentio.Punch
{
    [Serializable]
    [SequentioStep("Transform/Punch scale", "Punch scale")]
    public class ScalePunchStep : PunchStep<Transform>
    {
        protected override Vector3 GetValue(Transform target) => target.localScale;

        protected override void SetValue(Transform target, Vector3 value) => target.localScale = value;
    }
}