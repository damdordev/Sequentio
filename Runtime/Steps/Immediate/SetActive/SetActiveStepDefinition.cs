using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetActiveStepPlayer), "GameObject/Active", "Active", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetActiveStepDefinition : ValueImmediateStepDefinition<GameObject, bool>
    {
    }
}