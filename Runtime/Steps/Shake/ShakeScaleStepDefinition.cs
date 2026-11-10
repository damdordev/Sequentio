using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ShakeScaleStepPlayer), "Transform/Shake scale", "Shake scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakeScaleStepDefinition : ShakeStepDefinition<Transform>
    {
        public override void Reset()
        {
            base.Reset();
            Strength = new Vector3(0.2f, 0.2f, 0.0f);
        }
    }
}