using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting sprite on a SpriteRenderer.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererSpriteStepPlayer), "SpriteRenderer/Sprite", "Sprite", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetSpriteRendererSpriteStep_icon.png")]
    public class SetSpriteRendererSpriteStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, Sprite>
    {
    }
}
