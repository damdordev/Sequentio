using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Material.vector.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(MaterialVectorTweenStepPlayer), "Material/Vector", "Vector", "Packages/com.damdor.sequentio/Editor/StepsIcons/MaterialVectorTweenStep_icon.png")]
    public class MaterialVectorTweenStepDefinition : Vector4TweenStepDefinition<Material>
    {
        /// <summary>
        /// The name of the shader property to animate.
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
