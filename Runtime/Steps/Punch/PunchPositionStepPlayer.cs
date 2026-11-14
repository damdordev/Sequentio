using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the PunchPosition step.
    /// </summary>
    public class PunchPositionStepPlayer : PunchStepPlayer<PunchPositionStepDefinition, Transform>
    {
        private bool local;

        /// <inheritdoc />
        protected override void Init(PunchPositionStepDefinition definition, VarioStorage storage)
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
        protected override Vector3 GetValue(Transform component) => local ? component.localPosition : component.position;

        /// <inheritdoc />
        protected override void SetValue(Transform component, Vector3 value)
        {
            if (local) component.localPosition = value;
            else component.position = value;
        }
    }
}
