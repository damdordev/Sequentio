namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening integer values.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class IntTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, int>
        where TTweenStepDefinition : IntTweenStepDefinition<TComponent>
        where TComponent : class
    {
    }
}
