using UnityEngine.UI;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetImageSpriteStepPlayer : ValueImmediateStepPlayer<SetImageSpriteStepDefinition, Image, Sprite>
    {
        protected override Sprite GetValue(Image target) => target.sprite;
        protected override void SetValue(Image target, Sprite value) => target.sprite = value;
    }
}
