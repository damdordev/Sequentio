using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector2TweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Vector2>
        where TComponent : class
    {
        public VarioValue<Vector2AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }
        
        [SerializeField] private VarioValue<Vector2AxisFilter> axisFilter;

        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }
    }
}
