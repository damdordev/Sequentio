using System;
using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the sprite displayed by an image.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetImageSpriteStepPlayer), "Image/Sprite", "Sprite", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageSpriteStepDefinition : ValueImmediateStepDefinition<Image, Sprite>
    {
    }
}
