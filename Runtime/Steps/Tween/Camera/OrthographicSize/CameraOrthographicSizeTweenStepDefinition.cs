using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Camera.orthographicSize.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(CameraOrthographicSizeTweenStepPlayer), "Camera/OrthographicSize", "OrthographicSize", "Packages/com.damdor.sequentio/Editor/StepsIcons/CameraOrthographicSizeTweenStep_icon.png")]
    public class CameraOrthographicSizeTweenStepDefinition : FloatTweenStepDefinition<Camera>
    {
    }
}
