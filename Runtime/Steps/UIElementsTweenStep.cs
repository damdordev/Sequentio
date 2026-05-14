using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    public abstract class UIElementsTweenStep<TComponent, TValue> : BaseTweenStep<TComponent, UiToolkitQuery, TValue>
        where TComponent : VisualElement
    {
        protected override TComponent GetComponent(UiToolkitQuery pointer)
        {
            var root = SequentioInternalHelper.GetRoot(Storage);
            return root.Q<TComponent>(Target.Evaluate(Storage).Name);
        }
        
    }
}