using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable] 
    public class ShakeStepDefinition<TComponent> : StepDefinition
        where TComponent : class
    {
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
        
        public VarioValue<Vector3> Strength
        {
            get => strength;
            set => strength = value;
        }

        public VarioValue<int> Vibrato
        {
            get => vibrato;
            set => vibrato = value;
        }

        public VarioValue<bool> FadeOut
        {
            get => fadeOut;
            set => fadeOut = value;
        }

        public VarioValue<float> Randomness
        {
            get => randomness;
            set => randomness = value;
        }

        public VarioValue<ShakeRandomnessMode> RandomnessMode
        {
            get => randomnessMode;
            set => randomnessMode = value;
        }

        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<TComponent> target;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<float> duration;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<Vector3> strength;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<int> vibrato;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<bool> fadeOut;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<float> randomness;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<ShakeRandomnessMode> randomnessMode;
        
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        public override void Reset()
        {
            base.Reset();
            Target = null;
            duration = 1f;
            strength = new Vector3(2f, 2f, 0f);
            vibrato = 10;
            fadeOut = true;
            randomness = 90f;
            randomnessMode = ShakeRandomnessMode.Harmonic;
        }
    }
}