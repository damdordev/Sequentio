using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Material.keyword.
    /// </summary>
    public class SetMaterialKeywordStepPlayer : ValueImmediateStepPlayer<SetMaterialKeywordStepDefinition, Material, bool>
    {
        private string keyword;

        /// <inheritdoc />
        protected override void Init(SetMaterialKeywordStepDefinition definition, VarioStorage storage)
        {
            keyword = definition.Keyword.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            keyword = default;
        }

        /// <inheritdoc />
        protected override bool GetValue(Material component) => component.IsKeywordEnabled(keyword);
        /// <inheritdoc />
        protected override void SetValue(Material component, bool value)
        {
            var material = component;
            if (value) material.EnableKeyword(keyword);
            else material.DisableKeyword(keyword);
        }
    }
}
