using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Renderer.PropertyBlock/Color step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererPropertyBlockColorTweenStepPlayer), "Renderer.PropertyBlock/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/RendererPropertyBlockColorTweenStep_icon.png")]
    public class RendererPropertyBlockColorTweenStepDefinition : ColorTweenStepDefinition<Renderer>
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
