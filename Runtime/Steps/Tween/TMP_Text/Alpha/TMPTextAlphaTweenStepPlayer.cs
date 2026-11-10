using TMPro;

namespace Damdor.Sequentio
{
    public class TMPTextAlphaTweenStepPlayer : FloatTweenStepPlayer<TMPTextAlphaTweenStepDefinition, TMP_Text>
    {
        protected override float GetValue(TMP_Text component) => component.alpha;
        protected override void SetValue(TMP_Text component, float value) => component.alpha = value;
    }
}
