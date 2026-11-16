using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Renderer.Material.color.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererMaterialColorTweenStepPlayer), "Renderer.Material/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/RendererMaterialColorTweenStep_icon.png")]
    public class RendererMaterialColorTweenStepDefinition : ColorTweenStepDefinition<Renderer>
    {
        /// <summary>
        /// The main or custom color property to animate.
        /// </summary>
        public MaterialColorProperty Property
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
        
        [SerializeField] private MaterialColorProperty property;
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
