using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the PunchRotation step.
    /// </summary>
    public class PunchRotationStepPlayer : PunchStepPlayer<PunchRotationStepDefinition, Transform>
    {
        private bool local;

        /// <inheritdoc />
        protected override void Init(PunchRotationStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            local = default;
        }

        /// <inheritdoc />
        protected override Vector3 GetValue(Transform component) => local ? component.localEulerAngles : component.eulerAngles;

        /// <inheritdoc />
        protected override void SetValue(Transform component, Vector3 value)
        {
            if (local) component.localEulerAngles = value;
            else component.eulerAngles = value;
        }
    }
}
