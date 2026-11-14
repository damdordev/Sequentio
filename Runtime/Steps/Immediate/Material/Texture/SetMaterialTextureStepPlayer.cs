using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Material.texture.
    /// </summary>
    public class SetMaterialTextureStepPlayer : ValueImmediateStepPlayer<SetMaterialTextureStepDefinition, Material, Texture>
    {
        private MaterialTexturePropertyType propertyType;
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(SetMaterialTextureStepDefinition definition, VarioStorage storage)
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
        protected override Texture GetValue(Material component)
        {
            var material = component;
            return propertyType == MaterialTexturePropertyType.MainTexture
                ? material.mainTexture
                : material.GetTexture(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Material component, Texture value)
        {
            var material = component;
            if (propertyType == MaterialTexturePropertyType.MainTexture) material.mainTexture = value;
            else material.SetTexture(propertyName, value);
        }
    }
}
