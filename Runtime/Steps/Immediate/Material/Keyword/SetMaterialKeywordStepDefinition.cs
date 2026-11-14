using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Material.keyword.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetMaterialKeywordStepPlayer), "Material/Keyword", "Keyword", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetMaterialKeywordStepDefinition : ValueImmediateStepDefinition<Material, bool>
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

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            keyword = default;
        }
    }
}
