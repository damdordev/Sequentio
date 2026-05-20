using System;
using Damdor.Foundation;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/Position")]
    public class PositionTweenStep : TweenStep<Transform, Vector3>
    {
        public Vector3AxisFilter Filter
        {
            get => filter;
            set => filter = value;
        }
        
        protected override Vector3 GetValue(Transform target) => target.localPosition;

        protected override void SetValue(Transform target, Vector3 value)
            => target.localPosition = GetValue(target).Modify(value, filter);
        
        [SerializeField] private Vector3AxisFilter filter;
    }
}