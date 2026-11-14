using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Material.int.
    /// </summary>
    public class MaterialIntTweenStepPlayer : IntTweenStepPlayer<MaterialIntTweenStepDefinition, Material>
    {
        private string propertyName;

        /// <inheritdoc />
        protected override void Init(MaterialIntTweenStepDefinition definition, VarioStorage storage)
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
        protected override int GetValue(Material component) => component.GetInt(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Material component, int value) => component.SetInt(propertyName, value);
    }
}
