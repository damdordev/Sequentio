using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Animator/Float step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AnimatorFloatTweenStepPlayer), "Animator/Float", "Animator Float", "Packages/com.damdor.sequentio/Editor/StepsIcons/AnimatorFloatTweenStep_icon.png")]
    public class AnimatorFloatTweenStepDefinition : FloatTweenStepDefinition<Animator>
    {
        /// <summary>
        /// The Animator controller parameter to modify.
        /// </summary>
        public VarioValue<string> ParameterName
        {
            get => parameterName;
            set => parameterName = value;
        }

        [SerializeField] private VarioValue<string> parameterName;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            parameterName = default;
        }
    }
}
