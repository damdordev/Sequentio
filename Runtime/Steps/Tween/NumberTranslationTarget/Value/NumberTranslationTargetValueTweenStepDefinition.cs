#if DAMDOR_SEQUENTIO_TRADUIO

using Damdor.Traduio;
using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening the numeric value of a translation target.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(NumberTranslationTargetValueTweenStepPlayer), "NumberTranslationTarget/Value", "Value", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class NumberTranslationTargetValueTweenStepDefinition : FloatTweenStepDefinition<BaseNumberTranslationTarget>
    {
    }
}

#endif