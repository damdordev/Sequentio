using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the SetRendererPropertyBlockTexture step.
    /// </summary>
    public class SetRendererPropertyBlockTextureStepPlayer : ValueImmediateStepPlayer<SetRendererPropertyBlockTextureStepDefinition, Renderer, Texture>
    {
        private readonly RendererPropertyBlockState propertyBlock = new();
        private int propertyId;

        /// <inheritdoc />
        protected override void Init(SetRendererPropertyBlockTextureStepDefinition definition, VarioStorage storage)
        {
            propertyId = Shader.PropertyToID(definition.PropertyName.Evaluate(storage));
            propertyBlock.Capture(definition.Target.Evaluate(storage));
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            // A missing initial texture cannot be passed to MaterialPropertyBlock.SetTexture.
            // Restoring the block also preserves the absence of an original texture override.
            if (t < 0.5f) propertyBlock.Restore();
            else base.SetProgress(t);
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            // Restore the snapshot so originally absent overrides remain absent.
            propertyBlock.Restore();
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            propertyBlock.Reset();
            propertyId = default;
        }

        /// <inheritdoc />
        protected override Texture GetValue(Renderer component) => propertyBlock.GetTexture(component, propertyId);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Texture value) => propertyBlock.SetTexture(component, propertyId, value);
    }
}
