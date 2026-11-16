using System;
using UnityEngine;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the TMP_Text/MaxVisibleCharacters step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextMaxVisibleCharactersTweenStepPlayer), "TMP_Text/MaxVisibleCharacters", "MaxVisibleCharacters", "Packages/com.damdor.sequentio/Editor/StepsIcons/TMPTextMaxVisibleCharactersTweenStep_icon.png")]
    public class TMPTextMaxVisibleCharactersTweenStepDefinition : IntTweenStepDefinition<TMP_Text>
    {
    }
}
