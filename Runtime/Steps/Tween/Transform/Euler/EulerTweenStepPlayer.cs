using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening transform Euler angles in local or world coordinates.
    /// </summary>
    public class EulerTweenStepPlayer : Vector3TweenStepPlayer<EulerTweenStepDefinition, Transform>
    {
        private bool local;

        /// <inheritdoc />
        protected override void Init(EulerTweenStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
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