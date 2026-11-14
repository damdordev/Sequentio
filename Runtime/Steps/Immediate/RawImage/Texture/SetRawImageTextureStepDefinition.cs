using System;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the RawImage/Texture step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetRawImageTextureStepPlayer), "RawImage/Texture", "Texture", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetRawImageTextureStepDefinition : ValueImmediateStepDefinition<RawImage, Texture>
    {
    }
}
