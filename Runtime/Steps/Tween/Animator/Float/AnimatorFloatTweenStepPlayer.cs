using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the AnimatorFloatTween step.
    /// </summary>
    public class AnimatorFloatTweenStepPlayer : FloatTweenStepPlayer<AnimatorFloatTweenStepDefinition, Animator>
    {
        private int parameterId;

        /// <inheritdoc />
        protected override void Init(AnimatorFloatTweenStepDefinition definition, VarioStorage storage)
        {
            parameterId = Animator.StringToHash(definition.ParameterName.Evaluate(storage));
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            parameterId = default;
        }

        /// <inheritdoc />
        protected override float GetValue(Animator component) => component.GetFloat(parameterId);
        /// <inheritdoc />
        protected override void SetValue(Animator component, float value) => component.SetFloat(parameterId, value);
    }
}
