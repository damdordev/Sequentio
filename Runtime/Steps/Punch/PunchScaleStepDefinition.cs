using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for punching transform local scale.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(PunchScaleStepPlayer), "Transform/Punch scale", "Punch scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class PunchScaleStepDefinition : PunchStepDefinition<Transform>
    {
        
    }
}