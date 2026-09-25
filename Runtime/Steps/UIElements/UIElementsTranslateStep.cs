using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    [Serializable]
    // [SequentioStep("UiElements/Style/Translate/Value")]
    public class UIElementsTranslateStep : UIElementsVector3TweenStep<VisualElement>
    {
        protected override Vector3 GetValue(VisualElement target) => new(
            target.style.translate.value.x.value,
            target.style.translate.value.y.value,
            target.style.translate.value.z
        );

        protected override void SetValue(VisualElement target, Vector3 value) 
            => target.style.translate = value;
    }
}