using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening a UI graphic color.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(GraphicColorTweenStepPlayer), "Graphic/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class GraphicColorTweenStepDefinition : ColorTweenStepDefinition<Graphic>
    {
    }
}
