using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Material.color.
    /// </summary>
    public class MaterialColorTweenStepPlayer : ColorTweenStepPlayer<MaterialColorTweenStepDefinition, Material>
    {
        private MaterialColorPropertyType propertyType;
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(MaterialColorTweenStepDefinition definition, VarioStorage storage)
        {
            propertyType = definition.Property.Type;
            propertyName = propertyType == MaterialColorPropertyType.Custom ? definition.Property.Name.Evaluate(storage) : null;
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
        protected override Color GetValue(Material component)
        {
            var material = component;
            return propertyType == MaterialColorPropertyType.MainColor
                ? material.color
                : material.GetColor(propertyName);
        }
        /// <inheritdoc />
        protected override void SetValue(Material component, Color value)
        {
            var material = component;
            if (propertyType == MaterialColorPropertyType.MainColor) material.color = value;
            else material.SetColor(propertyName, value);
        }
    }
}
