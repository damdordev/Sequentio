using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Renderer.Material.vector.
    /// </summary>
    public class RendererMaterialVectorTweenStepPlayer : Vector4TweenStepPlayer<RendererMaterialVectorTweenStepDefinition, Renderer>
    {
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(RendererMaterialVectorTweenStepDefinition definition, VarioStorage storage)
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
        protected override Vector4 GetValue(Renderer component) => GetMaterial(component).GetVector(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Vector4 value) => GetMaterial(component).SetVector(propertyName, value);

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
