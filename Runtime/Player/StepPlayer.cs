using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public class StepPlayer
    {
        public virtual void Reset() { }
        public virtual void Init(VarioStorage storage) {}
        public virtual void SetProgress(float t){}
        public virtual void ReturnToInitialState(){}
        public virtual void Release(){}
    }
    
    public class StepPlayer<TStepDefinition> : StepPlayer where TStepDefinition : StepDefinition
    {
        private TStepDefinition definition;

        public void Setup(TStepDefinition definition)
        {
            this.definition = definition;
        }
        
        protected virtual void Init(TStepDefinition definition, VarioStorage storage){}

        public override void Init(VarioStorage storage)
        {
            base.Init(storage);
            Init(definition, storage);
        }

        public override void Reset()
        {
            base.Reset();
            definition = null;
        }
    }
}