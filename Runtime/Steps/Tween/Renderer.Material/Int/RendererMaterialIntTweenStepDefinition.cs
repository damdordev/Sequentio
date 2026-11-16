using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Renderer.Material.int.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RendererMaterialIntTweenStepPlayer), "Renderer.Material/Int", "Int", "Packages/com.damdor.sequentio/Editor/StepsIcons/RendererMaterialIntTweenStep_icon.png")]
    public class RendererMaterialIntTweenStepDefinition : IntTweenStepDefinition<Renderer>
    {
        /// <summary>
        /// The name of the shader property to animate.
        /// </summary>
        public VarioValue<string> PropertyName
        {
            get => propertyName;
            set => propertyName = value;
        }

        /// <summary>
        /// Whether to modify the shared material instead of the renderer material instance.
        /// </summary>
        public VarioValue<bool> SharedMaterial
        {
            get => sharedMaterial;
            set => sharedMaterial = value;
        }

        [SerializeField] private VarioValue<string> propertyName;
        [SerializeField] private VarioValue<bool> sharedMaterial;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyName = default;
            sharedMaterial = false;
        }
    }
}
