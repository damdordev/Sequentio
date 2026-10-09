using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public abstract class TweenStep<TComponent, TValue> : BaseTweenStep<TComponent, VarioValue<TComponent>, TValue>
        where TComponent : class
    {
        protected override TComponent GetComponent(VarioValue<TComponent> pointer) => pointer.Evaluate(Storage);

        public override string DynamicTitle => GetDynamicTitle();
        
        private string GetDynamicTitle()
        {
            var targetName = "";
            var component = GetComponent(Target) as Component;
            if (component != null) targetName = component.name;
            else if (Target.Source == ValueSource.Storage) targetName = ":" + Target.Name;

            return $"{targetName}, {Evaluate(Start)} -> {Evaluate(End)}, {Duration.Evaluate(Storage)}s";
        }
    }
}