using Damdor.Vario;
using UnityEngine.Events;

namespace Damdor.Sequentio
{
    public class RunEventStepPlayer : EventStepPlayer<RunEventStepDefinition>
    {
        private UnityEvent unityEvent;

        protected override void Init(RunEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            unityEvent = definition.Event;
        }

        protected override void FireEvent() => unityEvent?.Invoke();

        public override void Reset()
        {
            base.Reset();
            unityEvent = null;
        }
    }
}
