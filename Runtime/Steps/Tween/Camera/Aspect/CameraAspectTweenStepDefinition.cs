using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Camera.aspect.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(CameraAspectTweenStepPlayer), "Camera/Aspect", "Aspect", "Packages/com.damdor.sequentio/Editor/StepsIcons/CameraAspectTweenStep_icon.png")]
    public class CameraAspectTweenStepDefinition : FloatTweenStepDefinition<Camera>
    {
    }
}
