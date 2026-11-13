using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for shaking transform Euler angles in local or world coordinates.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(ShakeEulerStepPlayer), "Transform/Shake euler", "Shake rotation", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakeEulerStepDefinition : ShakeStepDefinition<Transform>
    {
        /// <summary>
        /// Whether to use local transform coordinates instead of world coordinates.
        /// </summary>
        public VarioValue<bool> Local
        {
            get => local;
            set => local = value;
        }
        
        [SerializeField] private VarioValue<bool> local;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            local = true;
            Strength = new Vector3(0f, 0f, 15f);
        }
    }
}