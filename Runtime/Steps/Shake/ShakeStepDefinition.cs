using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the target, duration, strength, and randomness of a shake.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    [Serializable] 
    public class ShakeStepDefinition<TComponent> : StepDefinition
        where TComponent : class
    {
        /// <summary>
        /// The target whose value this step modifies.
        /// </summary>
        public VarioValue<TComponent> Target
        {
            get => target;
            set => target = value;
        }

        /// <summary>
        /// The step duration in seconds, evaluated from sequence storage.
        /// </summary>
        public VarioValue<float> Duration
        {
            get => duration;
            set => duration = value;
        }
        
        /// <summary>
        /// The shake amplitude along each axis.
        /// </summary>
        public VarioValue<Vector3> Strength
        {
            get => strength;
            set => strength = value;
        }

        /// <summary>
        /// The vibration rate used with duration to determine the number of oscillation segments.
        /// </summary>
        public VarioValue<int> Vibrato
        {
            get => vibrato;
            set => vibrato = value;
        }

        /// <summary>
        /// Whether shake amplitude decreases toward the end of the step.
        /// </summary>
        public VarioValue<bool> FadeOut
        {
            get => fadeOut;
            set => fadeOut = value;
        }

        /// <summary>
        /// The angular randomness of the shake, in degrees.
        /// </summary>
        public VarioValue<float> Randomness
        {
            get => randomness;
            set => randomness = value;
        }

        /// <summary>
        /// The mode used to generate random shake directions.
        /// </summary>
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
        
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        /// <inheritdoc />
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