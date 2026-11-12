using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening alpha on a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextAlphaTweenStepPlayer), "TMP_Text/Alpha", "Alpha", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextAlphaTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
