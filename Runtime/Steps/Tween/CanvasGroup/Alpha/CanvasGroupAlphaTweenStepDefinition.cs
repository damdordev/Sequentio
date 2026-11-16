using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening canvas group opacity.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(CanvasGroupAlphaTweenStepPlayer), "CanvasGroup/Alpha", "Alpha", "Packages/com.damdor.sequentio/Editor/StepsIcons/CanvasGroupAlphaTweenStep_icon.png")]
    public class CanvasGroupAlphaTweenStepDefinition : FloatTweenStepDefinition<CanvasGroup>
    {
    }
}
