using System;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class FloatTweenStep<TComponent> : TweenStep<TComponent, float> 
        where TComponent : class
    {
    }
}