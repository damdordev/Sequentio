using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a zero-duration step that assigns a value to a target.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    /// <typeparam name="TValue">The type of value assigned or interpolated.</typeparam>
    [Serializable]
    public class ValueImmediateStepDefinition<TComponent, TValue> : StepDefinition
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
        /// The value to assign to the target.
        /// </summary>
        public VarioValue<TValue> Value
        {
            get => value;
            set => this.value = value;
        }

        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage) => 0f;

        [SerializeField] private VarioValue<TComponent> target;
        [SerializeField] private VarioValue<TValue> value;
    }
}