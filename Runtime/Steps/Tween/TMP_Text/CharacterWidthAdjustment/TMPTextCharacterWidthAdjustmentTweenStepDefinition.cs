using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(TMPTextCharacterWidthAdjustmentTweenStepPlayer), "TMP_Text/CharacterWidthAdjustment", "CharacterWidthAdjustment", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextCharacterWidthAdjustmentTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
