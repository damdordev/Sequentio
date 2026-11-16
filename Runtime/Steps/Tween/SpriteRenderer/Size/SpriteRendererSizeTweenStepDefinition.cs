using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening size on a SpriteRenderer.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SpriteRendererSizeTweenStepPlayer), "SpriteRenderer/Size", "Size", "Packages/com.damdor.sequentio/Editor/StepsIcons/SpriteRendererSizeTweenStep_icon.png")]
    public class SpriteRendererSizeTweenStepDefinition : Vector2TweenStepDefinition<SpriteRenderer>
    {
    }
}
