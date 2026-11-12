using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening transform position in local or world coordinates.
    /// </summary>
    [Serializable] 
    [SequentioStep(typeof(PositionTweenStepPlayer), "Transform/Position", "Move", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class PositionTweenStepDefinition : Vector3TweenStepDefinition<Transform>
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
        }
    }
}