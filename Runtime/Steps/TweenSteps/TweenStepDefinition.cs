using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class TweenStepDefinition<TComponent, TValue> : StepDefinition
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
        
        public VarioValue<AnimationCurve> Easing
        {
            get => easing;
            set => easing = value;
        }

        public TweenEndpoint<TComponent, TValue> Start
        {
            get => start;
            set => start = value;
        }
        
        public TweenEndpoint<TComponent, TValue> End
        {
            get => end;
            set => end = value;
        }

        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<TComponent> target;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<float> duration;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<AnimationCurve> easing;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private TweenEndpoint<TComponent, TValue> start;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private TweenEndpoint<TComponent, TValue> end;
        
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);
        
        private AnimationCurve defaultEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        public override void Reset()
        {
            base.Reset();
            duration = 1f;
            start = new TweenEndpoint<TComponent, TValue> { ValueSource = TweenPointSource.CurrentValue, Value = default };
            end = new TweenEndpoint<TComponent, TValue> { ValueSource = TweenPointSource.CurrentValue, Value = default };
            easing = defaultEasing;
        }

    }
}