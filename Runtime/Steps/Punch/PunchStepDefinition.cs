using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a damped punch using a direction, duration, vibration rate, and elasticity.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public class PunchStepDefinition<TComponent> : StepDefinition
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
        /// The initial punch offset and direction along each axis.
        /// </summary>
        public VarioValue<Vector3> Direction
        {
            get => direction;
            set => direction = value;
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
        /// The strength of oscillations opposite the punch direction, clamped to the range zero to one during preparation.
        /// </summary>
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
        
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            target = null;
            direction = new Vector3(0.2f, 0.2f, 0.2f);
            duration = 1f;
            vibrato = 10;
            elasticity = 1f;
        }
    }
}