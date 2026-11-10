using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetTMPTextTextStepPlayer), "TMP_Text/Text", "Text", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetTMPTextTextStepDefinition : ValueImmediateStepDefinition<TMP_Text, string>
    {
    }
}
