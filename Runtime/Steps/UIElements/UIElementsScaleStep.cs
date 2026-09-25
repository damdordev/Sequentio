using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    [Serializable]
    // [SequentioStep("UiElements/Style/Scale/Value")]
    public class UIElementsScaleStep : UIElementsVector3TweenStep<VisualElement>
    {
        protected override Vector3 GetValue(VisualElement target) =>
            target.style.scale.value.value;

        protected override void SetValue(VisualElement target, Vector3 value)
            => target.style.scale = value;
    }
}