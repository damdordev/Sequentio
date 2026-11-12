using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the active state of a game object.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetActiveStepPlayer), "GameObject/Active", "Active", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetActiveStepDefinition : ValueImmediateStepDefinition<GameObject, bool>
    {
    }
}