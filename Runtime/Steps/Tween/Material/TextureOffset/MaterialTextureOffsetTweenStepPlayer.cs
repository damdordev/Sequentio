using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Material.textureOffset.
    /// </summary>
    public class MaterialTextureOffsetTweenStepPlayer : Vector2TweenStepPlayer<MaterialTextureOffsetTweenStepDefinition, Material>
    {
        private MaterialTexturePropertyType propertyType;
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(MaterialTextureOffsetTweenStepDefinition definition, VarioStorage storage)
        {
            propertyType = definition.Property.Type;
            propertyName = propertyType == MaterialTexturePropertyType.Custom ? definition.Property.Name.Evaluate(storage) : null;
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyType = default;
            propertyName = default;
        }

        /// <inheritdoc />
        protected override Vector2 GetValue(Material component)
        {
            var material = component;
            return propertyType == MaterialTexturePropertyType.MainTexture
                ? material.mainTextureOffset
                : material.GetTextureOffset(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Material component, Vector2 value)
        {
            var material = component;
            if (propertyType == MaterialTexturePropertyType.MainTexture) material.mainTextureOffset = value;
            else material.SetTextureOffset(propertyName, value);
        }
    }
}
