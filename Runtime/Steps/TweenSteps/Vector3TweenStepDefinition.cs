using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class Vector3TweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, Vector3>
        where TComponent : class
    {
        public VarioValue<Vector3AxisFilter> AxisFilter
        {
            get => axisFilter;
            set => axisFilter = value;
        }
        
        [SerializeField] private VarioValue<Vector3AxisFilter> axisFilter;

        public override void Reset()
        {
            base.Reset();
            axisFilter = default;
        }
    }
}