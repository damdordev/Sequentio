using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Renderer.Material.keyword.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetRendererMaterialKeywordStepPlayer), "Renderer.Material/Keyword", "Keyword", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetRendererMaterialKeywordStepDefinition : ValueImmediateStepDefinition<Renderer, bool>
    {
        /// <summary>
        /// The shader keyword to enable or disable.
        /// </summary>
        public VarioValue<string> Keyword
        {
            get => keyword;
            set => keyword = value;
        }

        [SerializeField] private VarioValue<string> keyword;

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
            keyword = default;
            sharedMaterial = false;
        }
    }
}
