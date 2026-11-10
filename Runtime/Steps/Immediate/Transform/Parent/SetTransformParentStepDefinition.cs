using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetTransformParentStepPlayer), "Transform/Parent", "Parent", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetTransformParentStepDefinition : ValueImmediateStepDefinition<Transform, Transform>
    {
    }
}
