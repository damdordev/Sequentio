using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Renderer.PropertyBlock/Float step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererPropertyBlockFloatTweenStepPlayer), "Renderer.PropertyBlock/Float", "Float", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RendererPropertyBlockFloatTweenStepDefinition : FloatTweenStepDefinition<Renderer>
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
