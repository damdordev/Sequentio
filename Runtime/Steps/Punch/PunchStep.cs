using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio.Punch
{
    [Serializable]
    public abstract class PunchStep<TComponent> : SequenceStep 
        where TComponent : Object
    {
        public override string TargetNameInEditor => GetTargetName(target);
        
        public VarioValue<TComponent> Target
        {
            get => target;
            set => target = value;
        }

        public VarioValue<float> Duration
        {
            get => duration;
            set => duration = value;
        }
        
        public VarioValue<Vector3> Direction
        {
            get => direction;
            set => direction = value;
        }

        public VarioValue<int> Vibrato
        {
            get => vibrato;
            set => vibrato = value;
        }
        
        public VarioValue<float> Elasticity
        {
            get => elasticity;
            set => elasticity = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<TComponent> target;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<float> duration;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<Vector3> direction;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<int> vibrato;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<float> elasticity;

        private TComponent v_target;
        private int v_vibrato;
        private Vector3 v_direction;
        private float v_elasticity;
        
        private List<float> durations;
        private List<Vector3> endValues;
        private Vector3 initialValue;

        protected override float CalculateDuration() => duration.Evaluate(Storage);

        public override void Init()
        {
            base.Init();
            
            v_target = target.Evaluate(Storage);
            v_direction = direction.Evaluate(Storage);
            v_vibrato = vibrato.Evaluate(Storage);
            v_elasticity = elasticity.Evaluate(Storage);
            
            initialValue = GetValue(v_target);
            
            PrepareInternalLists();
        }

        public override void Release()
        {
            base.Release();
            
            if(durations != null) ListPool<float>.Release(durations);
            durations = null;
            if(endValues != null) ListPool<Vector3>.Release(endValues);
            endValues = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            
            SetValue(v_target, initialValue);
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            
            if(t <= 0f || t >= 1 || FullDuration <= Mathf.Epsilon)
            {
                SetValue(v_target, initialValue);
                return;
            }
            
            var index = 0;
            for(; index < durations.Count; index++)
            {
                if(durations[index] >= t) break;
            }
            
            var segmentStart = Math.Clamp(index - 1, 0, durations.Count - 1);
            var segmentEnd = Math.Clamp(index, 0, durations.Count - 1);
            var invertT = Mathf.InverseLerp(durations[segmentStart], durations[segmentEnd], t);
            var value = Vector3.LerpUnclamped(endValues[segmentStart], endValues[segmentEnd], invertT);
            SetValue(v_target, initialValue + value);
        }

        internal override void Reset()
        {
            base.Reset();
            duration = 0.5f;
            vibrato = 10;
            elasticity = 1f;
            direction = Vector3.zero;
        }

        private void PrepareInternalLists()
        {
            if (FullDuration < Mathf.Epsilon) return;
            
            // taken from DOTtween
            v_elasticity = Mathf.Clamp(v_elasticity, 0f, 1f);
            var magnitude = v_direction.magnitude;
            var length = Math.Max((int)(v_vibrato * FullDuration), 2);
            var num1 = magnitude / length;
            durations = ListPool<float>.Get();
            durations.Add(0f);
            for(var i = 0; i < length; ++i) durations.Add(0f);
            var num2 = 0.0f;
            for(var index = 0; index < length; ++index)
            {
                var num3 = (index + 1) / (float)length;
                var num4 = FullDuration * num3;
                num2 += num4;
                durations[index+1] = num4;
            }

            var num5 = FullDuration / num2;
            for(var index = 0; index < durations.Count; ++index)
                durations[index] *= num5;
            for(var i = 1; i < durations.Count; ++i) durations[i] += durations[i - 1];

            endValues = ListPool<Vector3>.Get();
            endValues.Add(Vector3.zero);
            for(var i = 0; i < length; ++i) endValues.Add(Vector3.zero);
            for(var index = 0; index < length; ++index)
            {
                if(index < length - 1)
                {
                    endValues[index+1] = index != 0
                        ? (index % 2 == 0
                            ? Vector3.ClampMagnitude(v_direction, magnitude)
                            : -Vector3.ClampMagnitude(v_direction, magnitude * v_elasticity))
                        : v_direction;
                    magnitude -= num1;
                }
                else
                    endValues[index+1] = Vector3.zero;

            }
        }

        protected abstract Vector3 GetValue(TComponent target);
        protected abstract void SetValue(TComponent target, Vector3 value);
    }
}