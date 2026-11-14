using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting Material.shader.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetMaterialShaderStepPlayer), "Material/Shader", "Shader", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetMaterialShaderStepDefinition : ValueImmediateStepDefinition<Material, Shader>
    {
    }
}
