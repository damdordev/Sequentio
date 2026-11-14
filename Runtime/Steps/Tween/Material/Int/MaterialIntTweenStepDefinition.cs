using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Material.int.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(MaterialIntTweenStepPlayer), "Material/Int", "Int", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class MaterialIntTweenStepDefinition : IntTweenStepDefinition<Material>
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
