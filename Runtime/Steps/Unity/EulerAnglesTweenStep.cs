using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/EulerAngles")]
    public class EulerAnglesTweenStep : Vector3TweenStep<Transform>
    {
        private bool v_global;
        
        protected override Vector3 GetValue(Transform target) => v_global ? target.localEulerAngles : target.eulerAngles;

        protected override void SetValue(Transform target, Vector3 value)
        {
            if (v_global) target.eulerAngles = value;
            else target.localEulerAngles = value;
        }

        public override void Init()
        {
            base.Init();
            v_global = global.Evaluate(Storage);
        }

        internal override void Reset()
        {
            base.Reset();
            global = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not PositionTweenStep other) return;
            global = other.global;
        }
        
        [SerializeField] private VarioValue<bool> global;
    }
}