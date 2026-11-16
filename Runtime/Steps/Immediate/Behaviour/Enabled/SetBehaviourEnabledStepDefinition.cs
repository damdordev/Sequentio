using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the enabled state of a behaviour.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetBehaviourEnabledStepPlayer), "Behaviour/Enabled", "Enabled", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetBehaviourEnabledStep_icon.png")]
    public class SetBehaviourEnabledStepDefinition : ValueImmediateStepDefinition<Behaviour, bool>
    {
    }
}
