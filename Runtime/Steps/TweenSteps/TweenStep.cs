using System;
using Damdor.Vario;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class TweenStep<TComponent, TValue> : BaseTweenStep<TComponent, VarioValue<TComponent>, TValue>
        where TComponent : Object
    {
        protected override TComponent GetComponent(VarioValue<TComponent> pointer) => pointer.Evaluate(Storage);

        public override string TargetNameInEditor => GetTargetName(Target);
        
    }
}