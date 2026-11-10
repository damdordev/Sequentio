using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class EulerTweenStepPlayer : Vector3TweenStepPlayer<EulerTweenStepDefinition, Transform>
    {
        private bool local;

        protected override void Init(EulerTweenStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        protected override Vector3 GetValue(Transform component) => local ? component.localEulerAngles : component.eulerAngles;

        protected override void SetValue(Transform component, Vector3 value)
        {
            if (local) component.localEulerAngles = value;
            else component.eulerAngles = value;
        }
    }
}