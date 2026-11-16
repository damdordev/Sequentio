using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Transform/PunchPosition step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(PunchPositionStepPlayer), "Transform/PunchPosition", "Punch Position", "Packages/com.damdor.sequentio/Editor/StepsIcons/PunchPositionStep_icon.png")]
    public class PunchPositionStepDefinition : PunchStepDefinition<Transform>
    {
        /// <summary>
        /// Whether to apply the punch in local coordinates.
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
