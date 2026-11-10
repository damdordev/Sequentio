using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(CanvasGroupAlphaTweenStepPlayer), "CanvasGroup/Alpha", "Alpha", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class CanvasGroupAlphaTweenStepDefinition : FloatTweenStepDefinition<CanvasGroup>
    {
    }
}
