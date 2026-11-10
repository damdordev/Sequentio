using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(PunchScaleStepPlayer), "Transform/Punch scale", "Punch scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class PunchScaleStepDefinition : PunchStepDefinition<Transform>
    {
        
    }
}