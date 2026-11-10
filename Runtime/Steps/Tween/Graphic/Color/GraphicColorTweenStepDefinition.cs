using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(GraphicColorTweenStepPlayer), "Graphic/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class GraphicColorTweenStepDefinition : ColorTweenStepDefinition<Graphic>
    {
    }
}
