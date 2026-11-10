namespace Damdor.Sequentio
{
    public abstract class FloatTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, float>
        where TTweenStepDefinition : FloatTweenStepDefinition<TComponent>
        where TComponent : class
    {
    }
}
