using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ShakeEulerStepPlayer), "Transform/Shake euler", "Shake rotation", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakeEulerStepDefinition : ShakeStepDefinition<Transform>
    {
        public VarioValue<bool> Local
        {
            get => local;
            set => local = value;
        }
        
        private VarioValue<bool> local;

        public override void Reset()
        {
            base.Reset();
            local = true;
            Strength = new Vector3(0f, 0f, 15f);
        }
    }
}