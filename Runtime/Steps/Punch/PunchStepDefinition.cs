using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class PunchStepDefinition<TComponent> : StepDefinition
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
        
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

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