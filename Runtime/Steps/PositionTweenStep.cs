using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/Position")]
    public class PositionTweenStep : TweenStep<Transform, Vector3>
    {
        protected override Vector3 GetValue(Transform target) => target.localPosition;
        protected override void SetValue(Transform target, Vector3 value) => target.localPosition = value;
    }
}