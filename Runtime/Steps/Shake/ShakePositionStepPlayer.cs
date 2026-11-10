using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class ShakePositionStepPlayer : ShakeStepPlayer<ShakePositionStepDefinition, Transform>
    {
        private bool local;

        protected override void Init(ShakePositionStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        protected override Vector3 GetValue(Transform target) => local ? target.localPosition : target.position;

        protected override void SetValue(Transform target, Vector3 value)
        {
            if(local) target.localPosition = value;
            else target.position = value;
        }

    }
}