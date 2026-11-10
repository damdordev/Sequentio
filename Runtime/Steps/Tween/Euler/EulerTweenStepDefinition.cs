using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable] 
    [SequentioStep(typeof(EulerTweenStepPlayer), "Transform/Euler", "Rotate (Euler)", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class EulerTweenStepDefinition : Vector3TweenStepDefinition<Transform>
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