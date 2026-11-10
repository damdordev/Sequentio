using System;
using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetImageSpriteStepPlayer), "Image/Sprite", "Sprite", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageSpriteStepDefinition : ValueImmediateStepDefinition<Image, Sprite>
    {
    }
}
