using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a target, duration, easing curve, and endpoints for a tween.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    /// <typeparam name="TValue">The type of value assigned or interpolated.</typeparam>
    [Serializable]
    public class TweenStepDefinition<TComponent, TValue> : StepDefinition
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
        /// The curve that maps normalized progress to the tween interpolation factor.
        /// </summary>
        public VarioValue<AnimationCurve> Easing
        {
            get => easing;
            set => easing = value;
        }

        /// <summary>
        /// The starting endpoint of the tween.
        /// </summary>
        public TweenEndpoint<TComponent, TValue> Start
        {
            get => start;
            set => start = value;
        }
        
        /// <summary>
        /// The ending endpoint of the tween.
        /// </summary>
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
        
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

        /// <inheritdoc />
        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);
        
        private AnimationCurve defaultEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        /// <inheritdoc />
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