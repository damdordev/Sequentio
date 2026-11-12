using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Applies damped oscillations in a configured direction relative to the captured initial value.
    /// </summary>
    /// <typeparam name="TStepDefinition">The step definition type handled by the player or compiler.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class PunchStepPlayer<TStepDefinition, TComponent> : StepPlayer<TStepDefinition>
        where TStepDefinition : PunchStepDefinition<TComponent>
        where TComponent : class
    {
        private TComponent target;
        private float duration;
        private Vector3 direction;
        private int vibrato;
        private float elasticity;

        private readonly List<float> durations = new();
        private readonly List<Vector3> endValues = new();
        private Vector3 initialValue;

        /// <inheritdoc />
        protected override void Init(TStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            duration = definition.Duration.Evaluate(storage);
            direction = definition.Direction.Evaluate(storage);
            vibrato = definition.Vibrato.Evaluate(storage);
            elasticity = definition.Elasticity.Evaluate(storage);
            
            initialValue = GetValue(target);
            Prepare();
        }

        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            if(t <= 0f || t >= 1)
            {
                SetValue(target, initialValue);
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
            SetValue(target, initialValue + value);
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initialValue);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            target = null;
            durations.Clear();
            endValues.Clear();
        }

        private void Prepare()
        {
            elasticity = Mathf.Clamp(elasticity, 0f, 1f);
            var magnitude = direction.magnitude;
            var length = Math.Max((int)(vibrato * duration), 2);
            var num1 = magnitude / length;
            for(var i = 0; i <= length; ++i) durations.Add(0f);
            var num2 = 0.0f;
            for(var index = 0; index < length; ++index)
            {
                var num3 = (index + 1) / (float)length;
                var num4 = duration * num3;
                num2 += num4;
                durations[index+1] = num4;
            }

            var num5 = duration / num2;
            for(var index = 0; index < durations.Count; ++index)
                durations[index] *= num5;
            for(var i = 1; i < durations.Count; ++i) durations[i] += durations[i - 1];
            
            for(var i = 0; i <= length; ++i) endValues.Add(Vector3.zero);
            for(var index = 0; index < length; ++index)
            {
                if(index < length - 1)
                {
                    endValues[index+1] = index != 0
                        ? (index % 2 == 0
                            ? Vector3.ClampMagnitude(direction, magnitude)
                            : -Vector3.ClampMagnitude(direction, magnitude * elasticity))
                        : direction;
                    magnitude -= num1;
                }
                else
                    endValues[index+1] = Vector3.zero;

            }
        }
        
        /// <summary>
        /// Reads the animated or assigned value from the target.
        /// </summary>
        /// <param name="target">The target whose value is read or written.</param>
        /// <returns>The current value of the target property.</returns>
        protected abstract Vector3 GetValue(TComponent target);
        /// <summary>
        /// Writes the animated or assigned value to the target.
        /// </summary>
        /// <param name="target">The target whose value is read or written.</param>
        /// <param name="value">The value to apply or convert.</param>
        protected abstract void SetValue(TComponent target, Vector3 value);
    }
}