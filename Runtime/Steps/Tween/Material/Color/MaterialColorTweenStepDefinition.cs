using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Material.color.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(MaterialColorTweenStepPlayer), "Material/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class MaterialColorTweenStepDefinition : ColorTweenStepDefinition<Material>
    {
        /// <summary>
        /// The main or custom color property to animate.
        /// </summary>
        public MaterialColorProperty Property
        {
            get => property;
            set => property = value;
        }

        [SerializeField] private MaterialColorProperty property;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            property = default;
        }
    }
}
