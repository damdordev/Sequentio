using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the RendererPropertyBlockColorTween step.
    /// </summary>
    public class RendererPropertyBlockColorTweenStepPlayer : ColorTweenStepPlayer<RendererPropertyBlockColorTweenStepDefinition, Renderer>
    {
        private readonly RendererPropertyBlockState propertyBlock = new();
        private int propertyId;

        /// <inheritdoc />
        protected override void Init(RendererPropertyBlockColorTweenStepDefinition definition, VarioStorage storage)
        {
            propertyId = Shader.PropertyToID(definition.PropertyName.Evaluate(storage));
            propertyBlock.Capture(definition.Target.Evaluate(storage));
            base.Init(definition, storage);
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
        protected override Color GetValue(Renderer component) => propertyBlock.GetColor(component, propertyId);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Color value) => propertyBlock.SetColor(component, propertyId, value);
    }
}
