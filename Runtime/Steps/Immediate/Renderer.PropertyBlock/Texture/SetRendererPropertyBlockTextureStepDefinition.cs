using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Renderer.PropertyBlock/Texture step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetRendererPropertyBlockTextureStepPlayer), "Renderer.PropertyBlock/Texture", "Texture", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetRendererPropertyBlockTextureStep_icon.png")]
    public class SetRendererPropertyBlockTextureStepDefinition : ValueImmediateStepDefinition<Renderer, Texture>
    {
        /// <summary>
        /// The shader property overridden on the renderer. The original renderer-level block is restored on return.
        /// </summary>
        public VarioValue<string> PropertyName
        {
            get => propertyName;
            set => propertyName = value;
        }

        [SerializeField] private VarioValue<string> propertyName;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyName = default;
        }
    }
}
