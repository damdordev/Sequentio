using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the AnimatorIntTween step.
    /// </summary>
    public class AnimatorIntTweenStepPlayer : IntTweenStepPlayer<AnimatorIntTweenStepDefinition, Animator>
    {
        private int parameterId;

        /// <inheritdoc />
        protected override void Init(AnimatorIntTweenStepDefinition definition, VarioStorage storage)
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
        protected override int GetValue(Animator component) => component.GetInteger(parameterId);
        /// <inheritdoc />
        protected override void SetValue(Animator component, int value) => component.SetInteger(parameterId, value);
    }
}
