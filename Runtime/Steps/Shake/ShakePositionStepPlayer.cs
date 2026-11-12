using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for shaking transform position in local or world coordinates.
    /// </summary>
    public class ShakePositionStepPlayer : ShakeStepPlayer<ShakePositionStepDefinition, Transform>
    {
        private bool local;

        /// <inheritdoc />
        protected override void Init(ShakePositionStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        protected override Vector3 GetValue(Transform target) => local ? target.localPosition : target.position;

        /// <inheritdoc />
        protected override void SetValue(Transform target, Vector3 value)
        {
            if(local) target.localPosition = value;
            else target.position = value;
        }

    }
}