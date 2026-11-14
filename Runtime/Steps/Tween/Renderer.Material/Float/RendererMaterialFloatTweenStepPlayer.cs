using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Renderer.Material.float.
    /// </summary>
    public class RendererMaterialFloatTweenStepPlayer : FloatTweenStepPlayer<RendererMaterialFloatTweenStepDefinition, Renderer>
    {
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(RendererMaterialFloatTweenStepDefinition definition, VarioStorage storage)
        {
            propertyName = definition.PropertyName.Evaluate(storage);
            sharedMaterial = definition.SharedMaterial.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyName = default;
            sharedMaterial = default;
        }

        /// <inheritdoc />
        protected override float GetValue(Renderer component) => GetMaterial(component).GetFloat(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, float value) => GetMaterial(component).SetFloat(propertyName, value);

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
