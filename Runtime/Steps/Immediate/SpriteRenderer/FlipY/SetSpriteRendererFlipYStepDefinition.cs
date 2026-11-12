using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting flip y on a SpriteRenderer.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererFlipYStepPlayer), "SpriteRenderer/FlipY", "FlipY", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetSpriteRendererFlipYStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, bool>
    {
    }
}
