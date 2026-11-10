using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Damdor.Sequentio
{
    public abstract class ShakeStepPlayer<TShakeStepDefinition, TComponent> : StepPlayer<TShakeStepDefinition>
        where TShakeStepDefinition : ShakeStepDefinition<TComponent>
        where TComponent : class
    {
        private TComponent target;
        private float duration;
        private Vector3 strength;
        private int vibrato;
        private bool fadeOut;
        private float randomness;
        private ShakeRandomnessMode randomnessMode;

        private readonly List<float> durations = new();
        private readonly List<Vector3> endValues = new();
        private Vector3 initialValue;
        
        protected override void Init(TShakeStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            duration = definition.Duration.Evaluate(storage);
            strength = definition.Strength.Evaluate(storage);
            vibrato = definition.Vibrato.Evaluate(storage);
            fadeOut = definition.FadeOut.Evaluate(storage);
            randomness = definition.Randomness.Evaluate(storage);
            randomnessMode = definition.RandomnessMode.Evaluate(storage);

            initialValue = GetValue(target);
            Prepare();
        }

        public override void Reset()
        {
            base.Reset();
            target = default;
            durations.Clear();
        }

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

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            SetValue(target, initialValue);
        }

        private void Prepare()
        {
            var num1 = strength.magnitude;
            var length = (int)(vibrato * duration);
            if(length < 2)
                length = 2;
            var num2 = num1 / length;
            durations.Clear();
            for(var i = 0; i < length; ++i) durations.Add(0f);

            var num3 = 0.0f;
            for(var index = 0; index < length; ++index)
            {
                var num4 = (index + 1) / (float)length;
                var num5 = fadeOut ? duration * num4 : duration / length;
                num3 += num5;
                durations[index] = num5;
            }

            var num6 = duration / num3;
            for(var index = 0; index < length; ++index) durations[index] = durations[index] * num6;
            for(var index = 1; index < length; ++index)  durations[index] += durations[index - 1];
            var degrees = Random.Range(0.0f, 360f);
            endValues.Clear();
            for(var i = 0; i < length; ++i) endValues.Add(Vector3.zero);

            for(var index = 0; index < length; ++index)
            {
                if(index < length - 1)
                {
                    var quaternion = Quaternion.identity;
                    if(randomnessMode == ShakeRandomnessMode.Harmonic)
                    {
                        if(index > 0) degrees = degrees - 180f + Random.Range(0.0f, randomness);
                        quaternion = Quaternion.AngleAxis(Random.Range(0.0f, randomness), Vector3.up);
                    }
                    else
                    {
                        if(index > 0) degrees = degrees - 180f + Random.Range(-randomness, randomness);
                        quaternion = Quaternion.AngleAxis(Random.Range(-randomness, randomness), Vector3.up);
                    }

                    var vector = quaternion * Vector3FromAngle(degrees, num1);
                    vector.x = Vector3.ClampMagnitude(vector, strength.x).x;
                    vector.y = Vector3.ClampMagnitude(vector, strength.y).y;
                    vector.z = Vector3.ClampMagnitude(vector, strength.z).z;
                    vector = vector.normalized * num1;
                    endValues[index] = vector;
                    if(fadeOut) num1 -= num2;
                    strength = Vector3.ClampMagnitude(strength, num1);
                }
                else
                    endValues[index] = Vector3.zero;
            }
        }
        
        private static Vector3 Vector3FromAngle(float degrees, float magnitude)
        {
            var f = degrees * (Mathf.PI / 180f);
            return new Vector3(magnitude * Mathf.Cos(f), magnitude * Mathf.Sin(f), 0.0f);
        }
        
        protected abstract Vector3 GetValue(TComponent target);
        protected abstract void SetValue(TComponent target, Vector3 value);
    }
}