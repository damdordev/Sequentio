using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Material.float.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(MaterialFloatTweenStepPlayer), "Material/Float", "Float", "Packages/com.damdor.sequentio/Editor/StepsIcons/MaterialFloatTweenStep_icon.png")]
    public class MaterialFloatTweenStepDefinition : FloatTweenStepDefinition<Material>
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
