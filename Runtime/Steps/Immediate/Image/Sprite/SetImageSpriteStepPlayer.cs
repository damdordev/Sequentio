using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the sprite displayed by an image.
    /// </summary>
    public class SetImageSpriteStepPlayer : ValueImmediateStepPlayer<SetImageSpriteStepDefinition, Image, Sprite>
    {
        /// <inheritdoc />
        protected override Sprite GetValue(Image target) => target.sprite;
        /// <inheritdoc />
        protected override void SetValue(Image target, Sprite value) => target.sprite = value;
    }
}
