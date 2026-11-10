using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SpriteRendererColorTweenStepPlayer), "SpriteRenderer/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SpriteRendererColorTweenStepDefinition : ColorTweenStepDefinition<SpriteRenderer>
    {
    }
}
