using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting flip x on a SpriteRenderer.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererFlipXStepPlayer), "SpriteRenderer/FlipX", "FlipX", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetSpriteRendererFlipXStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, bool>
    {
    }
}
