using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Renderer.Material.shader.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetRendererMaterialShaderStepPlayer), "Renderer.Material/Shader", "Shader", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetRendererMaterialShaderStep_icon.png")]
    public class SetRendererMaterialShaderStepDefinition : ValueImmediateStepDefinition<Renderer, Shader>
    {
        /// <summary>
        /// Whether to modify the shared material instead of the renderer material instance.
        /// </summary>
        public VarioValue<bool> SharedMaterial
        {
            get => sharedMaterial;
            set => sharedMaterial = value;
        }

        [SerializeField] private VarioValue<bool> sharedMaterial;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            sharedMaterial = false;
        }
    }
}
