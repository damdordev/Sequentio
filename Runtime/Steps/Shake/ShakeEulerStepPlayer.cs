using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class ShakeEulerStepPlayer : ShakeStepPlayer<ShakeEulerStepDefinition, Transform>
    {
        private bool local;

        protected override void Init(ShakeEulerStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }
        
        protected override Vector3 GetValue(Transform target) => local ? target.localEulerAngles : target.eulerAngles;

        protected override void SetValue(Transform target, Vector3 value)
        {
            if(local) target.localEulerAngles = value;
            else target.eulerAngles = value;
        }
    }
}