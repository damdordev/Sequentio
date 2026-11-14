using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Material.textureScale.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(MaterialTextureScaleTweenStepPlayer), "Material/TextureScale", "TextureScale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class MaterialTextureScaleTweenStepDefinition : Vector2TweenStepDefinition<Material>
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

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            property = default;
        }
    }
}
