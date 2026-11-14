using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Renderer.Material.texture.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetRendererMaterialTextureStepPlayer), "Renderer.Material/Texture", "Texture", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetRendererMaterialTextureStepDefinition : ValueImmediateStepDefinition<Renderer, Texture>
    {
        /// <summary>
        /// The main or custom texture property to modify.
        /// </summary>
        public MaterialTextureProperty Property
        {
            get => property;
            set => property = value;
        }

        [SerializeField] private MaterialTextureProperty property;

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
            property = default;
            sharedMaterial = false;
        }
    }
}
