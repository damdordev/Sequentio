using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Renderer.Material.textureOffset.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererMaterialTextureOffsetTweenStepPlayer), "Renderer.Material/TextureOffset", "TextureOffset", "Packages/com.damdor.sequentio/Editor/StepsIcons/RendererMaterialTextureOffsetTweenStep_icon.png")]
    public class RendererMaterialTextureOffsetTweenStepDefinition : Vector2TweenStepDefinition<Renderer>
    {
        /// <summary>
        /// The main or custom texture property to modify.
        /// </summary>
        public MaterialTextureProperty Property
        {
            get => property;
            set => property = value;
        }

        /// <summary>
        /// Whether to modify the shared material instead of the renderer material instance.
        /// </summary>
        public VarioValue<bool> SharedMaterial
        {
            get => sharedMaterial;
            set => sharedMaterial = value;
        }

        [SerializeField] private MaterialTextureProperty property;
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
