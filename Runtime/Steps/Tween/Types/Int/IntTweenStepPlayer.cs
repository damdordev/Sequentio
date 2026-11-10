namespace Damdor.Sequentio
{
    public abstract class IntTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, int>
        where TTweenStepDefinition : IntTweenStepDefinition<TComponent>
        where TComponent : class
    {
    }
}
