using System;
using System.Collections.Generic;
using Damdor.Vario;
using NUnit.Framework;

namespace Damdor.Sequentio.Tests
{
    public class MockStepTarget
    {
        public float Value;
    }

    public class MockStepDefinition : StepDefinition
    {
        public VarioValue<float> Duration = VarioValue<float>.Raw(2f);
        public Exception DurationException;
        public int ResetCalls;
        public MockStepTarget Target;
        public float TargetValue;
        public string Label;
        public List<string> CallOrder;

        public override float CalculateDuration(VarioStorage storage)
        {
            if (DurationException != null) throw DurationException;
            return Duration.Evaluate(storage);
        }

        public override void Reset()
        {
            base.Reset();
            ResetCalls++;
        }
    }

    public class UnregisteredStepDefinition : MockStepDefinition { }

    public class MockStepPlayer : StepPlayer<MockStepDefinition>
    {
        public float LastProgress = -1f;
        public readonly List<float> ProgressCalls = new();
        public int InitCalls;
        public int ReturnCalls;
        public MockStepDefinition InitializedDefinition;
        public VarioStorage InitializedStorage;
        public float InitialValue;
        public Exception InitException;
        public Exception ProgressException;
        public Exception ReturnException;
        public Action OnReturn;

        protected override void Init(MockStepDefinition definition, VarioStorage storage)
        {
            InitCalls++;
            InitializedDefinition = definition;
            InitializedStorage = storage;
            definition.CallOrder?.Add(definition.Label + ".Init");
            if (InitException != null) throw InitException;
            if (definition.Target != null) InitialValue = definition.Target.Value;
        }

        public override void SetProgress(float t)
        {
            LastProgress = t;
            ProgressCalls.Add(t);
            InitializedDefinition?.CallOrder?.Add(InitializedDefinition.Label + ".Progress");
            if (ProgressException != null) throw ProgressException;
            if (InitializedDefinition?.Target != null)
                InitializedDefinition.Target.Value = InitialValue + (InitializedDefinition.TargetValue - InitialValue) * t;
        }

        public override void ReturnToInitialState()
        {
            LastProgress = 0f;
            ReturnCalls++;
            InitializedDefinition?.CallOrder?.Add(InitializedDefinition.Label + ".Return");
            OnReturn?.Invoke();
            if (ReturnException != null) throw ReturnException;
            if (InitializedDefinition?.Target != null) InitializedDefinition.Target.Value = InitialValue;
        }

        public override void Reset()
        {
            base.Reset();
            LastProgress = -1f;
            ProgressCalls.Clear();
            InitCalls = ReturnCalls = 0;
            InitializedDefinition = null;
            InitializedStorage = null;
            InitialValue = 0f;
            InitException = ProgressException = ReturnException = null;
            OnReturn = null;
        }
    }

    // Unity runs these tests sequentially. Restore global error reporting after every test.
    public abstract class SequenceTestBase
    {
        protected SequenceCompiler Compiler;
        protected readonly List<SequentioStepErrorHandlerData> Errors = new();
        private readonly List<Action> cleanup = new();
        private SequentioStepErrorHandler previousHandler;
        private bool previousLogging;

        [SetUp]
        public void SetUpSequenceTest()
        {
            Compiler = new SequenceCompiler();
            Compiler.RegisterStep<MockStepDefinition, MockStepPlayer>();
            previousHandler = SequentioSettings.StepErrorHandler;
            previousLogging = SequentioSettings.LogStepErrors;
            Errors.Clear();
            SequentioSettings.LogStepErrors = false;
            SequentioSettings.StepErrorHandler = Errors.Add;
        }

        [TearDown]
        public void TearDownSequenceTest()
        {
            try
            {
                for (var i = cleanup.Count - 1; i >= 0; i--) cleanup[i]();
            }
            finally
            {
                cleanup.Clear();
                SequentioSettings.StepErrorHandler = previousHandler;
                SequentioSettings.LogStepErrors = previousLogging;
            }
        }

        protected SequencePlan Plan(ISequenceDefinition definition, SequenceSettings settings = null,
            Action<VarioStorage, SequenceSettings> modify = null)
        {
            var plan = Compiler.CreateDefinitionPlan(definition, settings ?? new SequenceSettings(), modify);
            cleanup.Add(plan.Release);
            return plan;
        }

        protected SequenceExecution Execute(SequencePlan plan)
        {
            var execution = Compiler.CreateExecution(plan);
            cleanup.Add(execution.Release);
            return execution;
        }

        protected SequenceExecutor Executor(SequenceExecution execution)
        {
            var executor = SequenceExecutor.Create(execution);
            cleanup.Add(executor.Release);
            return executor;
        }

        protected static void AssertProgress(StepExecution step, float expected)
        {
            Assert.That(step.Progress, Is.EqualTo(expected).Within(0.00001f), "StepExecution.Progress");
            Assert.That(((MockStepPlayer)step.Player).LastProgress,
                Is.EqualTo(expected).Within(0.00001f), "StepPlayer.SetProgress argument");
        }

        protected void AssertError(SequentioStepErrorSource source, StepDefinition definition,
            StepPlayer player, Exception exception)
        {
            Assert.That(Errors, Has.Count.EqualTo(1));
            Assert.That(Errors[0].ErrorSource, Is.EqualTo(source));
            Assert.That(Errors[0].Definition, Is.SameAs(definition));
            Assert.That(Errors[0].Player, Is.SameAs(player));
            Assert.That(Errors[0].Exception, Is.SameAs(exception));
        }
    }
}
