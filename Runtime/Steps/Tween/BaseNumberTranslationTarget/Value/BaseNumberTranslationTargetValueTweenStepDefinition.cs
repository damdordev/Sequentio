using Damdor.Traduio;
using System;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(BaseNumberTranslationTargetValueTweenStepPlayer), "BaseNumberTranslationTarget/Value", "Value", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class BaseNumberTranslationTargetValueTweenStepDefinition : FloatTweenStepDefinition<BaseNumberTranslationTarget>
    {
    }
}
