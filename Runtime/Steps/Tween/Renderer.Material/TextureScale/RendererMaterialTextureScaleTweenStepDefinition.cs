using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Renderer.Material.textureScale.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererMaterialTextureScaleTweenStepPlayer), "Renderer.Material/TextureScale", "TextureScale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RendererMaterialTextureScaleTweenStepDefinition : Vector2TweenStepDefinition<Renderer>
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
