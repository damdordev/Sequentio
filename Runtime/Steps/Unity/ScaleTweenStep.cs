using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Transform/Scale", "Scale", "Packages/com.damdor.sequentio/Editor/StepsIcons/scale.png")]
    public class ScaleTweenStep : TweenStep<Transform, Vector3>
    {
        protected override Vector3 GetValue(Transform target) => target.localScale;
        protected override void SetValue(Transform target, Vector3 value) => target.localScale = value;
    } 
}