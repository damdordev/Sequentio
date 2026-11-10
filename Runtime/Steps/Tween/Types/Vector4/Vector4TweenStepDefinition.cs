using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector4TweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Vector4>
        where TComponent : class
    {
        public VarioValue<Vector4AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }
        
        [SerializeField] private VarioValue<Vector4AxisFilter> axisFilter;

        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }
    }
}
