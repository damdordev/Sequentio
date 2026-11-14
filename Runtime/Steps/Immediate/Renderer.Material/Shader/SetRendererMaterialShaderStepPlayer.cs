using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Renderer.Material.shader.
    /// </summary>
    public class SetRendererMaterialShaderStepPlayer : ValueImmediateStepPlayer<SetRendererMaterialShaderStepDefinition, Renderer, Shader>
    {
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(SetRendererMaterialShaderStepDefinition definition, VarioStorage storage)
        {
            sharedMaterial = definition.SharedMaterial.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            sharedMaterial = default;
        }

        /// <inheritdoc />
        protected override Shader GetValue(Renderer component) => GetMaterial(component).shader;
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Shader value) => GetMaterial(component).shader = value;

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
