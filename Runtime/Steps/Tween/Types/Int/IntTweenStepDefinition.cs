using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening integer values.
    /// </summary>
    /// <typeparam name="TComponent">The target type whose value is modified.</typeparam>
    [Serializable]
    public abstract class IntTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, int>
        where TComponent : class
    {
    }
}
