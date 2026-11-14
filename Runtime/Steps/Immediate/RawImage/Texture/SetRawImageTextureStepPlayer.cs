using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the SetRawImageTexture step.
    /// </summary>
    public class SetRawImageTextureStepPlayer : ValueImmediateStepPlayer<SetRawImageTextureStepDefinition, RawImage, Texture>
    {
        /// <inheritdoc />
        protected override Texture GetValue(RawImage component) => component.texture;
        /// <inheritdoc />
        protected override void SetValue(RawImage component, Texture value) => component.texture = value;
    }
}
