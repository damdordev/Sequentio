using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Camera.fieldOfView.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(CameraFieldOfViewTweenStepPlayer), "Camera/FieldOfView", "FieldOfView", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class CameraFieldOfViewTweenStepDefinition : FloatTweenStepDefinition<Camera>
    {
    }
}
