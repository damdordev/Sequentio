using UnityEngine;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the TMPTextMaxVisibleCharactersTween step.
    /// </summary>
    public class TMPTextMaxVisibleCharactersTweenStepPlayer : IntTweenStepPlayer<TMPTextMaxVisibleCharactersTweenStepDefinition, TMP_Text>
    {
        /// <inheritdoc />
        protected override int GetValue(TMP_Text component) => component.maxVisibleCharacters;
        /// <inheritdoc />
        protected override void SetValue(TMP_Text component, int value) => component.maxVisibleCharacters = value;
    }
}
