namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening floating-point values.
    /// </summary>
    /// <typeparam name="TTweenStepDefinition">The tween definition type handled by the player.</typeparam>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    public abstract class FloatTweenStepPlayer<TTweenStepDefinition, TComponent> : TweenStepPlayer<TTweenStepDefinition, TComponent, float>
        where TTweenStepDefinition : FloatTweenStepDefinition<TComponent>
        where TComponent : class
    {
    }
}
