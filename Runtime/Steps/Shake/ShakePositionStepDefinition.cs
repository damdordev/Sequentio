using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ShakePositionStepPlayer), "Transform/Shake position", "Shake position", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakePositionStepDefinition : ShakeStepDefinition<Transform>
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
        }
    }
}