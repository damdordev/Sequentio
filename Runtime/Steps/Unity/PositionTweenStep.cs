using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/Position", "Move", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class PositionTweenStep : Vector3TweenStep<Transform>
    {
        public VarioValue<bool> Global
        {
            get => global;
            set => global = value;
        }
        
        private bool v_global;
        
        protected override Vector3 GetValue(Transform target) => v_global ? target.position : target.localPosition;

        protected override void SetValue(Transform target, Vector3 value)
        {
            if (v_global) target.position = value;
            else target.localPosition = value;
        }

        public override void Init()
        {
            v_global = global.Evaluate(Storage);
            base.Init();
        }

        internal override void Reset()
        {
            base.Reset();
            Global = false;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not PositionTweenStep other) return;
            global = other.global;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Default, -1)] private VarioValue<bool> global;
    }
}