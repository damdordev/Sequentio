using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
     [Serializable]
     [SequentioStep(typeof(PositionTweenStepPlayer), "Transform/Position", "Move", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class PositionTweenStepDefinition : Vector3TweenStepDefinition<Transform>
    {
        public VarioValue<bool> Local
        {
            get => local;
            set => local = value;
        }
        
        [SerializeField] private VarioValue<bool> local;

        public override void Reset()
        {
            base.Reset();
            local = true;
        }
    }
}