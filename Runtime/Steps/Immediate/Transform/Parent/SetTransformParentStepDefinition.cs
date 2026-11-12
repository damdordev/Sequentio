using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the parent of a transform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetTransformParentStepPlayer), "Transform/Parent", "Parent", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetTransformParentStepDefinition : ValueImmediateStepDefinition<Transform, Transform>
    {
    }
}
