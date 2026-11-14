using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Renderer.Material.int.
    /// </summary>
    public class RendererMaterialIntTweenStepPlayer : IntTweenStepPlayer<RendererMaterialIntTweenStepDefinition, Renderer>
    {
        private string propertyName;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(RendererMaterialIntTweenStepDefinition definition, VarioStorage storage)
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
        protected override int GetValue(Renderer component) => GetMaterial(component).GetInt(propertyName);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, int value) => GetMaterial(component).SetInt(propertyName, value);

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
