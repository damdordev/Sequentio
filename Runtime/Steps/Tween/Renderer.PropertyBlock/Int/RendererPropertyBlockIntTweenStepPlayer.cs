using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the RendererPropertyBlockIntTween step.
    /// </summary>
    public class RendererPropertyBlockIntTweenStepPlayer : IntTweenStepPlayer<RendererPropertyBlockIntTweenStepDefinition, Renderer>
    {
        private readonly RendererPropertyBlockState propertyBlock = new();
        private int propertyId;

        /// <inheritdoc />
        protected override void Init(RendererPropertyBlockIntTweenStepDefinition definition, VarioStorage storage)
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
        protected override int GetValue(Renderer component) => propertyBlock.GetInt(component, propertyId);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, int value) => propertyBlock.SetInt(component, propertyId, value);
    }
}
