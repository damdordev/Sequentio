using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class PositionTweenStep : TweenStep<Transform, Vector3>
    {
        protected override Vector3 GetValue(Transform target) => target.localPosition;
        protected override void SetValue(Transform target, Vector3 value) => target.localPosition = value;
        protected override Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
        protected override Vector3 Sum(Vector3 a, Vector3 b) => a + b;
    }
}