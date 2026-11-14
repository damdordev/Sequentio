using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Camera.backgroundColor.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(CameraBackgroundColorTweenStepPlayer), "Camera/BackgroundColor", "BackgroundColor", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class CameraBackgroundColorTweenStepDefinition : ColorTweenStepDefinition<Camera>
    {
    }
}
