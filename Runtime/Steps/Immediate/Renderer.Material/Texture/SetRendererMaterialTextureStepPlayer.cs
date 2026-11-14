using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Renderer.Material.texture.
    /// </summary>
    public class SetRendererMaterialTextureStepPlayer : ValueImmediateStepPlayer<SetRendererMaterialTextureStepDefinition, Renderer, Texture>
    {
        private MaterialTexturePropertyType propertyType;
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(SetRendererMaterialTextureStepDefinition definition, VarioStorage storage)
        {
            propertyType = definition.Property.Type;
            propertyName = propertyType == MaterialTexturePropertyType.Custom ? definition.Property.Name.Evaluate(storage) : null;
            sharedMaterial = definition.SharedMaterial.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyType = default;
            propertyName = default;
            sharedMaterial = default;
        }

        /// <inheritdoc />
        protected override Texture GetValue(Renderer component)
        {
            var material = GetMaterial(component);
            return propertyType == MaterialTexturePropertyType.MainTexture
                ? material.mainTexture
                : material.GetTexture(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Texture value)
        {
            var material = GetMaterial(component);
            if (propertyType == MaterialTexturePropertyType.MainTexture) material.mainTexture = value;
            else material.SetTexture(propertyName, value);
        }

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
