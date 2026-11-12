using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Describes a tween endpoint as an explicit value or an offset from the current or referenced target value.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    /// <typeparam name="TValue">The type of value assigned or interpolated.</typeparam>
    [Serializable]
    public struct TweenEndpoint<TComponent, TValue>
    {
        /// <summary>
        /// The source used to evaluate the tween endpoint.
        /// </summary>
        public TweenPointSource ValueSource
        {
            get => valueSource;
            set => valueSource = value;
        }

        /// <summary>
        /// The target whose current value is used when the endpoint source is a reference target.
        /// </summary>
        public VarioValue<TComponent> OtherTarget
        {
            get => otherTarget;
            set => otherTarget = value;
        }
        
        /// <summary>
        /// The explicit endpoint value, or the offset added to the current or referenced target value.
        /// </summary>
        public VarioValue<TValue> Value
        {
            get => value;
            set => this.value = value;
        }
        
        [SerializeField] private TweenPointSource valueSource;
        [SerializeField] private VarioValue<TComponent> otherTarget;
        [SerializeField] private VarioValue<TValue> value;
            
        /// <summary>
        /// Creates an explicit tween endpoint from a value.
        /// </summary>
        /// <param name="value">The value to apply or convert.</param>
        /// <returns>An endpoint whose source is ExplicitValue and whose value is the supplied value.</returns>
        public static implicit operator TweenEndpoint<TComponent, TValue>(TValue value) => new() { ValueSource = TweenPointSource.ExplicitValue, Value = value };

    }
}