using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening color on a SpriteRenderer.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SpriteRendererColorTweenStepPlayer), "SpriteRenderer/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SpriteRendererColorTweenStepDefinition : ColorTweenStepDefinition<SpriteRenderer>
    {
    }
}
