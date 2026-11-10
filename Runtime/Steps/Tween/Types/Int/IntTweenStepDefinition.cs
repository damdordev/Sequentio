using System;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class IntTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, int>
        where TComponent : class
    {
    }
}
