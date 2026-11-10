using TMPro;

namespace Damdor.Sequentio
{
    public class SetTMPTextTextStepPlayer : ValueImmediateStepPlayer<SetTMPTextTextStepDefinition, TMP_Text, string>
    {
        protected override string GetValue(TMP_Text target) => target.text;
        protected override void SetValue(TMP_Text target, string value) => target.text = value;
    }
}
