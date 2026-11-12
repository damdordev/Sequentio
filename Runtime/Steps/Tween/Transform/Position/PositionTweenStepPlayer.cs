using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening transform position in local or world coordinates.
    /// </summary>
    public class PositionTweenStepPlayer : Vector3TweenStepPlayer<PositionTweenStepDefinition, Transform>
    {
        private bool local;

        // ReSharper disable Unity.PerformanceAnalysis
        /// <inheritdoc />
        protected override void Init(PositionTweenStepDefinition definition, VarioStorage storage)
        {
            local = definition.Local.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            local = true;
        }
        
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform component) => local ? component.localPosition : component.position;

        /// <inheritdoc />
        protected override void SetValue(Transform component, Vector3 value)
        {
            if (local) component.localPosition = value;
            else component.position = value;
        }
    }
}