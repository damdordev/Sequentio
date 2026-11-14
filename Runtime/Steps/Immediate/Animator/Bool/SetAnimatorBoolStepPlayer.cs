using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the SetAnimatorBool step.
    /// </summary>
    public class SetAnimatorBoolStepPlayer : ValueImmediateStepPlayer<SetAnimatorBoolStepDefinition, Animator, bool>
    {
        private int parameterId;

        /// <inheritdoc />
        protected override void Init(SetAnimatorBoolStepDefinition definition, VarioStorage storage)
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
        protected override bool GetValue(Animator component) => component.GetBool(parameterId);
        /// <inheritdoc />
        protected override void SetValue(Animator component, bool value) => component.SetBool(parameterId, value);
    }
}
