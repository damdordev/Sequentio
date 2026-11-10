using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable] 
    [SequentioStep(typeof(ScaleTweenStepPlayer), "Transform/Scale", "Scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ScaleTweenStepDefinition : Vector3TweenStepDefinition<Transform>
    {
    }
}