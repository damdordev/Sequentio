using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetBehaviourEnabledStepPlayer), "Behaviour/Enabled", "Enabled", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetBehaviourEnabledStepDefinition : ValueImmediateStepDefinition<Behaviour, bool>
    {
    }
}
