using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Animator/Int step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AnimatorIntTweenStepPlayer), "Animator/Int", "Int", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class AnimatorIntTweenStepDefinition : IntTweenStepDefinition<Animator>
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
