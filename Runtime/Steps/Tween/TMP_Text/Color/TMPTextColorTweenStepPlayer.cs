using TMPro;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class TMPTextColorTweenStepPlayer : ColorTweenStepPlayer<TMPTextColorTweenStepDefinition, TMP_Text>
    {
        protected override Color GetValue(TMP_Text component) => component.color;
        protected override void SetValue(TMP_Text component, Color value) => component.color = value;
    }
}
