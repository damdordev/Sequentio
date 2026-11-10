using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(TMPTextCharacterHorizontalScaleTweenStepPlayer), "TMP_Text/CharacterHorizontalScale", "CharacterHorizontalScale", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextCharacterHorizontalScaleTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
