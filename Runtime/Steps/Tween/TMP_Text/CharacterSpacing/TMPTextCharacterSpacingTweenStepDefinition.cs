using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(TMPTextCharacterSpacingTweenStepPlayer), "TMP_Text/CharacterSpacing", "CharacterSpacing", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextCharacterSpacingTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
