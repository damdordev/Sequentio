using System;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class FloatTweenStep<TComponent> : TweenStep<TComponent, float> 
        where TComponent : Object
    {
    }
}