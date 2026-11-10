using System;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class FloatTweenStepDefinition<TComponent> : TweenStepDefinition<TComponent, float>
        where TComponent : class
    {
    }
}
