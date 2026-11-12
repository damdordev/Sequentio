using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening two-dimensional vector values.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    [Serializable]
    public abstract class Vector2TweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Vector2>
        where TComponent : class
    {
        /// <summary>
        /// The vector axes modified by the tween.
        /// </summary>
        public VarioValue<Vector2AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }
        
        [SerializeField] private VarioValue<Vector2AxisFilter> axisFilter;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }
    }
}
