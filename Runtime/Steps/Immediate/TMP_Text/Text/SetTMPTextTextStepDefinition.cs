using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the text of a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetTMPTextTextStepPlayer), "TMP_Text/Text", "Text", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetTMPTextTextStepDefinition : ValueImmediateStepDefinition<TMP_Text, string>
    {
    }
}
