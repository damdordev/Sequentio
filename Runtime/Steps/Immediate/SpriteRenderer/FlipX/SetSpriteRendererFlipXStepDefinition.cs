using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererFlipXStepPlayer), "SpriteRenderer/FlipX", "FlipX", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetSpriteRendererFlipXStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, bool>
    {
    }
}
