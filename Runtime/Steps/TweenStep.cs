using Damdor.Vario;

namespace Damdor.Sequentio
{
    public abstract class TweenStep<TComponent, TValue> : BaseTweenStep<TComponent, VarioValue<TComponent>, TValue>
    {
        protected override TComponent GetComponent(VarioValue<TComponent> pointer) => pointer.Evaluate(Storage);
    }
}