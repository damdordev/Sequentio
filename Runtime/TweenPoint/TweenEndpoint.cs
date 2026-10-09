using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public struct TweenEndpoint<TComponent, TValue>
    {
        public TweenPointSource ValueSource
        {
            get => valueSource;
            set => valueSource = value;
        }

        public VarioValue<TComponent> OtherTarget
        {
            get => otherTarget;
            set => otherTarget = value;
        }
        
        public VarioValue<TValue> Value
        {
            get => value;
            set => this.value = value;
        }
        
        [SerializeField] private TweenPointSource valueSource;
        [SerializeField] private VarioValue<TComponent> otherTarget;
        [SerializeField] private VarioValue<TValue> value;
            
        public static implicit operator TweenEndpoint<TComponent, TValue>(TValue value) => new() { ValueSource = TweenPointSource.ExplicitValue, Value = value };

    }
}