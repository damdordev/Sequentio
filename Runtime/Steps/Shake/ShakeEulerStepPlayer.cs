using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for shaking transform Euler angles in local or world coordinates.
    /// </summary>
    public class ShakeEulerStepPlayer : ShakeStepPlayer<ShakeEulerStepDefinition, Transform>
    {
        private bool local;

        /// <inheritdoc />
        protected override void Init(ShakeEulerStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }
        
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform target) => local ? target.localEulerAngles : target.eulerAngles;

        /// <inheritdoc />
        protected override void SetValue(Transform target, Vector3 value)
        {
            if(local) target.localEulerAngles = value;
            else target.eulerAngles = value;
        }
    }
}