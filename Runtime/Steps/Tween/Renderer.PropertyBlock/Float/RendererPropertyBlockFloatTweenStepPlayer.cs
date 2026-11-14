using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the RendererPropertyBlockFloatTween step.
    /// </summary>
    public class RendererPropertyBlockFloatTweenStepPlayer : FloatTweenStepPlayer<RendererPropertyBlockFloatTweenStepDefinition, Renderer>
    {
        private readonly RendererPropertyBlockState propertyBlock = new();
        private int propertyId;

        /// <inheritdoc />
        protected override void Init(RendererPropertyBlockFloatTweenStepDefinition definition, VarioStorage storage)
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
        protected override float GetValue(Renderer component) => propertyBlock.GetFloat(component, propertyId);
        /// <inheritdoc />
        protected override void SetValue(Renderer component, float value) => propertyBlock.SetFloat(component, propertyId, value);
    }
}
