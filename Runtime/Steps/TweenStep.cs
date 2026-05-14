namespace Damdor.Sequentio
{
    public abstract class TweenStep<TComponent, TValue> : BaseTweenStep<TComponent, TComponent, TValue>
    {
        protected override TComponent GetComponent(TComponent pointer) => pointer;
    }
}