using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the RendererPropertyBlockVectorTween step.
    /// </summary>
    public class RendererPropertyBlockVectorTweenStepPlayer : Vector4TweenStepPlayer<RendererPropertyBlockVectorTweenStepDefinition, Renderer>
    {
        private readonly RendererPropertyBlockState propertyBlock = new();
        private int propertyId;

        /// <inheritdoc />
        protected override void Init(RendererPropertyBlockVectorTweenStepDefinition definition, VarioStorage storage)
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
        protected override Vector4 GetValue(Renderer component) => propertyBlock.GetVector(component, propertyId);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, Vector4 value) => propertyBlock.SetVector(component, propertyId, value);
    }
}
