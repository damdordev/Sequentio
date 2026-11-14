using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Renderer.Material.color.
    /// </summary>
    public class RendererMaterialColorTweenStepPlayer : ColorTweenStepPlayer<RendererMaterialColorTweenStepDefinition, Renderer>
    {
        private MaterialColorPropertyType propertyType;
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(RendererMaterialColorTweenStepDefinition definition, VarioStorage storage)
        {
            propertyType = definition.Property.Type;
            propertyName = propertyType == MaterialColorPropertyType.Custom ? definition.Property.Name.Evaluate(storage) : null;
            sharedMaterial = definition.SharedMaterial.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyType = default;
            propertyName = null;
            sharedMaterial = false;
        }

        /// <inheritdoc />
        protected override Color GetValue(Renderer component)
        {
            var material = GetMaterial(component);
            return propertyType == MaterialColorPropertyType.MainColor
                ? material.color
                : material.GetColor(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Color value)
        {
            var material = GetMaterial(component);
            if (propertyType == MaterialColorPropertyType.MainColor) material.color = value;
            else material.SetColor(propertyName, value);
        }

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
