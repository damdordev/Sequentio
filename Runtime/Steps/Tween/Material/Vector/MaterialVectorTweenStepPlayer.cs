using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Material.vector.
    /// </summary>
    public class MaterialVectorTweenStepPlayer : Vector4TweenStepPlayer<MaterialVectorTweenStepDefinition, Material>
    {
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(MaterialVectorTweenStepDefinition definition, VarioStorage storage)
        {
            propertyName = definition.PropertyName.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyName = default;
        }

        /// <inheritdoc />
        protected override Vector4 GetValue(Material component) => component.GetVector(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Material component, Vector4 value) => component.SetVector(propertyName, value);
    }
}
