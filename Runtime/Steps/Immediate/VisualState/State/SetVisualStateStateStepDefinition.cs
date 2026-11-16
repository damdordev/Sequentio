using System;
using Damdor.Statio;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetVisualStateStateStepPlayer), "VisualState/State", "State", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetVisualStateStateStepDefinition : ValueImmediateStepDefinition<VisualState, string>
    {
        public VarioValue<bool> Immediate
        {
            get => immediate;
            set => immediate = value;
        }

        [SerializeField] private VarioValue<bool> immediate = false;

        public override void Reset()
        {
            base.Reset();
            immediate = false;
        }
    }
}
