using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Material.float.
    /// </summary>
    public class MaterialFloatTweenStepPlayer : FloatTweenStepPlayer<MaterialFloatTweenStepDefinition, Material>
    {
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(MaterialFloatTweenStepDefinition definition, VarioStorage storage)
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
        protected override float GetValue(Material component) => component.GetFloat(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Material component, float value) => component.SetFloat(propertyName, value);
    }
}
