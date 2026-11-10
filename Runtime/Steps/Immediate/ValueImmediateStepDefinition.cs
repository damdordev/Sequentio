using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class ValueImmediateStepDefinition<TComponent, TValue> : StepDefinition
        where TComponent : class
    {
        public VarioValue<TComponent> Target
        {
            get => target;
            set => target = value;
        }
        
        public VarioValue<TValue> Value
        {
            get => value;
            set => this.value = value;
        }

        public override float CalculateDuration(VarioStorage storage) => 0f;

        [SerializeField] private VarioValue<TComponent> target;
        [SerializeField] private VarioValue<TValue> value;
    }
}