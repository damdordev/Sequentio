using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting Material.shader.
    /// </summary>
    public class SetMaterialShaderStepPlayer : ValueImmediateStepPlayer<SetMaterialShaderStepDefinition, Material, Shader>
    {
        /// <inheritdoc />
        protected override Shader GetValue(Material component) => component.shader;
        /// <inheritdoc />
        protected override void SetValue(Material component, Shader value) => component.shader = value;
    }
}
