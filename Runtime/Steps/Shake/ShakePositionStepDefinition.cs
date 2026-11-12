using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for shaking transform position in local or world coordinates.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(ShakePositionStepPlayer), "Transform/Shake position", "Shake position", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ShakePositionStepDefinition : ShakeStepDefinition<Transform>
    {
        /// <summary>
        /// Whether to use local transform coordinates instead of world coordinates.
        /// </summary>
        public VarioValue<bool> Local
        {
            get => local;
            set => local = value;
        }
        
        private VarioValue<bool> local;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            local = true;
        }
    }
}