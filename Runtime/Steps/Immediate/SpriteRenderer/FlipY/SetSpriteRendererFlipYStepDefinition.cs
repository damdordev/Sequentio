using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererFlipYStepPlayer), "SpriteRenderer/FlipY", "FlipY", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetSpriteRendererFlipYStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, bool>
    {
    }
}
