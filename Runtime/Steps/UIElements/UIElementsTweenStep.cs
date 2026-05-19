using Damdor.Vario;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    public abstract class UIElementsTweenStep<TComponent, TValue> : BaseTweenStep<TComponent, UIElementsPointer<TComponent>, TValue>
        where TComponent : VisualElement
    {
        protected override TComponent GetComponent(UIElementsPointer<TComponent> pointer)
        {
            return pointer.Evaluate(Storage);
        }
        
    }
}