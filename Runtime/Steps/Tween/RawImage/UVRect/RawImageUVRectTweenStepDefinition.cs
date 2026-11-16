using System;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the RawImage/UVRect step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RawImageUVRectTweenStepPlayer), "RawImage/UVRect", "UVRect", "Packages/com.damdor.sequentio/Editor/StepsIcons/RawImageUVRectTweenStep_icon.png")]
    public class RawImageUVRectTweenStepDefinition : TweenStepDefinition<RawImage, Rect>
    {
    }
}
