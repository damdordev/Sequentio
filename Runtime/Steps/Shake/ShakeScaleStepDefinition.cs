using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for shaking transform local scale.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(ShakeScaleStepPlayer), "Transform/Shake scale", "Shake scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakeScaleStepDefinition : ShakeStepDefinition<Transform>
    {
        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            Strength = new Vector3(0.2f, 0.2f, 0.0f);
        }
    }
}