using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio.RunSequence
{
    public class RunSequenceStepPlayer : StepPlayer<RunSequenceStepDefinition>
    {
        private SequenceExecution execution;
        private SequenceExecutor executor;

        protected override void Init(RunSequenceStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            var sequenceDefinition = definition.Sequence.Evaluate(storage);
            var plan = SequenceCompiler.CreateDefinitionPlan(sequenceDefinition, true, null);
            execution = SequenceCompiler.CreateExecution(plan);
            executor = SequenceExecutor.Create(execution);
            plan.Release();
        }

        public override void Release()
        {
            base.Release();
            executor?.Release();
            executor = null;
            
            execution?.Release();
            execution = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            executor.ReturnToInitialState();
            executor.ReleaseSteps();
        }
        
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            executor.SetTime(t*execution.Duration);
        }
        
    }
}