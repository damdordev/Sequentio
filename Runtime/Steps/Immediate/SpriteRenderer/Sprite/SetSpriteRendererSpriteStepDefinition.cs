using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetSpriteRendererSpriteStepPlayer), "SpriteRenderer/Sprite", "Sprite", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetSpriteRendererSpriteStepDefinition : ValueImmediateStepDefinition<SpriteRenderer, Sprite>
    {
    }
}
