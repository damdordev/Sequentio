using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/EulerAngles", "Rotate")]
    public class EulerAnglesTweenStep : Vector3TweenStep<Transform>
    {
        public VarioValue<bool> Global
        {
            get => global;
            set => global = value;
        }
        
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
            Global = default;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not EulerAnglesTweenStep other) return;
            Global = other.Global;
        }
        
        [SerializeField] private VarioValue<bool> global;
    }
}