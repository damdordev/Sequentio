using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Material.texture.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetMaterialTextureStepPlayer), "Material/Texture", "Texture", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetMaterialTextureStepDefinition : ValueImmediateStepDefinition<Material, Texture>
    {
        /// <summary>
        /// The main or custom texture property to modify.
        /// </summary>
        public MaterialTextureProperty Property
        {
            get => property;
            set => property = value;
        }

        [SerializeField] private MaterialTextureProperty property;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            property = default;
        }
    }
}
