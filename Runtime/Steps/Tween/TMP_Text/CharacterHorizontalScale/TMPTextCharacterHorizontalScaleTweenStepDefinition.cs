using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening character horizontal scale on a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextCharacterHorizontalScaleTweenStepPlayer), "TMP_Text/CharacterHorizontalScale", "CharacterHorizontalScale", "Packages/com.damdor.sequentio/Editor/StepsIcons/TMPTextCharacterHorizontalScaleTweenStep_icon.png")]
    public class TMPTextCharacterHorizontalScaleTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
