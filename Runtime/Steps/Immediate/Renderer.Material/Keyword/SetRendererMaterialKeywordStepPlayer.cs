using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Renderer.Material.keyword.
    /// </summary>
    public class SetRendererMaterialKeywordStepPlayer : ValueImmediateStepPlayer<SetRendererMaterialKeywordStepDefinition, Renderer, bool>
    {
        private string keyword;
        private bool sharedMaterial;

        /// <inheritdoc />
        protected override void Init(SetRendererMaterialKeywordStepDefinition definition, VarioStorage storage)
        {
            keyword = definition.Keyword.Evaluate(storage);
            sharedMaterial = definition.SharedMaterial.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            keyword = default;
            sharedMaterial = default;
        }

        /// <inheritdoc />
        protected override bool GetValue(Renderer component) => GetMaterial(component).IsKeywordEnabled(keyword);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, bool value)
        {
            var material = GetMaterial(component);
            if (value) material.EnableKeyword(keyword);
            else material.DisableKeyword(keyword);
        }

        private Material GetMaterial(Renderer component) => sharedMaterial ? component.sharedMaterial : component.material;
    }
}
