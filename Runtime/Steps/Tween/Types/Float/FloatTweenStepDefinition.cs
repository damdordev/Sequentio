using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening floating-point values.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    [Serializable]
    public abstract class FloatTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, float>
        where TComponent : class
    {
    }
}
