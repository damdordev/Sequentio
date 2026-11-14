using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Renderer.Material.textureOffset.
    /// </summary>
    public class RendererMaterialTextureOffsetTweenStepPlayer : Vector2TweenStepPlayer<RendererMaterialTextureOffsetTweenStepDefinition, Renderer>
    {
        private MaterialTexturePropertyType propertyType;
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(RendererMaterialTextureOffsetTweenStepDefinition definition, VarioStorage storage)
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
        protected override Vector2 GetValue(Renderer component)
        {
            var material = GetMaterial(component);
            return propertyType == MaterialTexturePropertyType.MainTexture
                ? material.mainTextureOffset
                : material.GetTextureOffset(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Vector2 value)
        {
            var material = GetMaterial(component);
            if (propertyType == MaterialTexturePropertyType.MainTexture) material.mainTextureOffset = value;
            else material.SetTextureOffset(propertyName, value);
        }

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
