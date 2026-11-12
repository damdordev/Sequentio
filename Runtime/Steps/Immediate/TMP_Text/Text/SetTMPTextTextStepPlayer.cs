using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the text of a TextMesh Pro component.
    /// </summary>
    public class SetTMPTextTextStepPlayer : ValueImmediateStepPlayer<SetTMPTextTextStepDefinition, TMP_Text, string>
    {
        /// <inheritdoc />
        protected override string GetValue(TMP_Text target) => target.text;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text target, string value) => target.text = value;
    }
}
