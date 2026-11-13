using System;
using Damdor.Vario;
using NUnit.Framework;

namespace Damdor.Sequentio.Tests
{
    public class SequenceCompilerTests : SequenceTestBase
    {
        [Test]
        public void RegisterStep_CreateStepPlayer_CreatesPlayerBoundToDefinition()
        {
            var definition = new SequenceDefinition().AddStep<MockStepDefinition>();
            var player = Compiler.CreateStepPlayer(definition);
            Assert.That(player, Is.TypeOf<MockStepPlayer>());
            var storage = new VarioStorage();
            player.Init(storage);
            Assert.That(((MockStepPlayer)player).InitializedDefinition, Is.SameAs(definition));
            Assert.That(((MockStepPlayer)player).InitializedStorage, Is.SameAs(storage));
        }

        [Test]
        public void CreateStepPlayer_UnregisteredDefinition_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Compiler.CreateStepPlayer(new UnregisteredStepDefinition()));
        }

        // Fixed expected timelines cover different durations/delays and both longest-member positions.
        [Test]
        public void CreateDefinitionPlan_CalculatesTimelineAndCopiesParameters(
            [Values(0, 1, 2, 3)] int layout,
            [Values(false, true)] bool fromStorage,
            [Values(0.25f, 1.5f)] float delay,
            [Values(false, true)] bool firstIsLongest)
        {
            var definition = new SequenceDefinition();
            var count = layout == 0 ? 1 : layout == 3 ? 3 : 2;
            var durations = firstIsLongest ? new[] { 4f, 2f, 3f } : new[] { 2f, 4f, 3f };
            var delays = new[] { delay, delay + 0.5f, delay };
            for (var i = 0; i < count; i++)
            {
                var step = definition.AddStep<MockStepDefinition>();
                definition.Storage.Update("duration" + i, durations[i]);
                definition.Storage.Update("delay" + i, delays[i]);
                step.Duration = fromStorage ? VarioValue<float>.FromStorage("duration" + i) : VarioValue<float>.Raw(durations[i]);
                step.Delay = fromStorage ? VarioValue<float>.FromStorage("delay" + i) : VarioValue<float>.Raw(delays[i]);
                step.RunWithPreviousStep = layout >= 2 && i == 1;
                step.InitOnStart = i % 2 == 0;
            }

            var plan = Plan(definition);
            var firstEnd = delay + durations[0];
            var secondStart = layout >= 2 ? delays[1] : firstEnd + delays[1];
            var secondEnd = secondStart + durations[1];
            var groupEnd = firstIsLongest ? firstEnd : secondEnd;
            var starts = new[] { delay, secondStart, groupEnd + delay };
            var ends = new[] { firstEnd, secondEnd, groupEnd + delay + 3f };
            Assert.That(plan.Steps, Has.Count.EqualTo(count));
            Assert.That(plan.Duration, Is.EqualTo(count == 1 ? firstEnd : layout == 3 ? ends[2] : layout == 2 ? groupEnd : secondEnd));
            for (var i = 0; i < count; i++)
            {
                var step = plan.Steps[i];
                Assert.That(step.Definition, Is.SameAs(definition.Steps[i]));
                Assert.That(step.Duration, Is.EqualTo(durations[i]));
                Assert.That(step.Delay, Is.EqualTo(delays[i]));
                Assert.That(step.StartTime, Is.EqualTo(starts[i]));
                Assert.That(step.EndTime, Is.EqualTo(ends[i]));
                Assert.That(step.InitOnStart, Is.EqualTo(i % 2 == 0));
                Assert.That(step.RunWithPreviousStep, Is.EqualTo(layout >= 2 && i == 1));
                Assert.That(step.OriginalIndex, Is.EqualTo(i));
                Assert.That(step.IndexInGroup, Is.EqualTo(layout >= 2 && i == 1 ? 1 : 0));
            }
        }

        [Test]
        public void CreateDefinitionPlan_EmptyDefinition_HasNoStepsAndZeroDuration()
        {
            var plan = Plan(new SequenceDefinition());
            Assert.That(plan.Steps, Is.Empty);
            Assert.That(plan.Duration, Is.Zero);
        }

        [Test]
        public void CreateDefinitionPlan_FirstStepRunsWithPrevious_StartsFirstGroupAtZero()
        {
            var definition = new SequenceDefinition();
            var step = definition.AddStep<MockStepDefinition>();
            step.RunWithPreviousStep = true;
            step.Delay = 0.5f;
            var plan = Plan(definition);
            Assert.That(plan.Steps[0].StartTime, Is.EqualTo(0.5f));
            Assert.That(plan.Steps[0].EndTime, Is.EqualTo(2.5f));
            Assert.That(plan.Steps[0].IndexInGroup, Is.Zero);
            Assert.That(plan.Duration, Is.EqualTo(2.5f));
        }

        [Test]
        public void CreateDefinitionPlan_AllStepsDisabled_HasNoStepsAndZeroDuration()
        {
            var definition = new SequenceDefinition();
            definition.AddStep<MockStepDefinition>().Enabled = false;
            definition.AddStep<MockStepDefinition>().Enabled = false;
            var plan = Plan(definition);
            Assert.That(plan.Steps, Is.Empty);
            Assert.That(plan.Duration, Is.Zero);
            Assert.That(Errors, Is.Empty);
        }

        [Test]
        public void CreateExecution_EmptyPlan_HasNoStepsAndZeroDuration()
        {
            var execution = Execute(Plan(new SequenceDefinition()));
            Assert.That(execution.Steps, Is.Empty);
            Assert.That(execution.Duration, Is.Zero);
            Assert.That(execution.CurrentTime, Is.Zero);
        }

        [Test]
        public void CreateDefinitionPlan_DisabledSteps_AreExcludedBeforeDurationEvaluation(
            [Values(false, true)] bool fromStorage)
        {
            var definition = new SequenceDefinition();
            var disabled = definition.AddStep<MockStepDefinition>();
            definition.Storage.Update("enabled", false);
            disabled.Enabled = fromStorage ? VarioValue<bool>.FromStorage("enabled") : VarioValue<bool>.Raw(false);
            disabled.DurationException = new InvalidOperationException("Must not calculate disabled step");
            var enabled = definition.AddStep<MockStepDefinition>();
            var plan = Plan(definition);
            Assert.That(plan.Steps, Has.Count.EqualTo(1));
            Assert.That(plan.Steps[0].Definition, Is.SameAs(enabled));
            Assert.That(plan.Steps[0].OriginalIndex, Is.EqualTo(1));
            Assert.That(plan.Steps[0].StartTime, Is.Zero);
            Assert.That(plan.Duration, Is.EqualTo(2f));
            Assert.That(Errors, Is.Empty);
        }

        private static SequenceSettings NonDefaultSettings() => new()
        {
            AutoPlay = AutoPlayMode.OnEnable, Loop = LoopMode.PingPong,
            Timescale = TimescaleMode.Unscaled, Speed = 3f,
            Direction = PlayDirection.Reverse, DefaultStopBehaviour = StopBehaviour.LeaveInCurrentState
        };

        private static void AssertSettingsCopied(SequenceSettings copy, SequenceSettings original)
        {
            Assert.That(copy, Is.Not.SameAs(original));
            Assert.That(copy.AutoPlay, Is.EqualTo(original.AutoPlay));
            Assert.That(copy.Loop, Is.EqualTo(original.Loop));
            Assert.That(copy.Timescale, Is.EqualTo(original.Timescale));
            Assert.That(copy.Speed, Is.EqualTo(original.Speed));
            Assert.That(copy.Direction, Is.EqualTo(original.Direction));
            Assert.That(copy.DefaultStopBehaviour, Is.EqualTo(original.DefaultStopBehaviour));
        }

        [Test]
        public void CreateDefinitionPlan_ClonesSettingsAndStorage()
        {
            var definition = new SequenceDefinition();
            definition.Storage.Update("value", 10);
            var settings = NonDefaultSettings();
            var plan = Plan(definition, settings);
            AssertSettingsCopied(plan.Settings, settings);
            Assert.That(plan.Storage, Is.Not.SameAs(definition.Storage));
            Assert.That(plan.Storage.Get<int>("value"), Is.EqualTo(10));
            plan.Settings.Speed = 7f;
            plan.Storage.Update("value", 20);
            Assert.That(settings.Speed, Is.EqualTo(3f));
            Assert.That(definition.Storage.Get<int>("value"), Is.EqualTo(10));
        }

        [Test]
        public void CreateDefinitionPlan_ModifyCallback_ChangesClonesBeforeCalculatingDuration()
        {
            var definition = new SequenceDefinition();
            definition.Storage.Update("duration", 2f);
            definition.AddStep<MockStepDefinition>().Duration = VarioValue<float>.FromStorage("duration");
            var settings = new SequenceSettings();
            var plan = Plan(definition, settings, (storage, copy) =>
            {
                storage.Update("duration", 5f);
                copy.Speed = 3f;
            });
            Assert.That(plan.Duration, Is.EqualTo(5f));
            Assert.That(plan.Settings.Speed, Is.EqualTo(3f));
            Assert.That(definition.Storage.Get<float>("duration"), Is.EqualTo(2f));
            Assert.That(settings.Speed, Is.EqualTo(1f));
        }

        [Test]
        public void CreateDefinitionPlan_DurationException_ReportsAndOmitsStepWithoutAffectingNeighbours()
        {
            var definition = new SequenceDefinition();
            var first = definition.AddStep<MockStepDefinition>();
            var broken = definition.AddStep<MockStepDefinition>();
            var last = definition.AddStep<MockStepDefinition>();
            broken.DurationException = new InvalidOperationException("duration failed");
            SequencePlan plan = null;
            Assert.DoesNotThrow(() => plan = Plan(definition));
            Assert.That(plan.Steps.ConvertAll(s => s.Definition), Is.EqualTo(new[] { first, last }));
            Assert.That(plan.Steps[1].StartTime, Is.EqualTo(2f));
            Assert.That(plan.Steps[1].OriginalIndex, Is.EqualTo(2));
            Assert.That(plan.Duration, Is.EqualTo(4f));
            Assert.That(definition.Steps, Has.Count.EqualTo(3));
            AssertError(SequentioStepErrorSource.CalculateDuration, broken, null, broken.DurationException);
        }

        [Test]
        public void CreateExecution_ClonesSettingsAndStorage()
        {
            var definition = new SequenceDefinition();
            definition.Storage.Update("value", 10);
            var plan = Plan(definition, NonDefaultSettings());
            var execution = Execute(plan);
            AssertSettingsCopied(execution.Settings, plan.Settings);
            Assert.That(execution.Storage, Is.Not.SameAs(plan.Storage));
            Assert.That(execution.Storage.Get<int>("value"), Is.EqualTo(10));
            execution.Settings.Speed = 9f;
            execution.Storage.Update("value", 20);
            Assert.That(plan.Settings.Speed, Is.EqualTo(3f));
            Assert.That(plan.Storage.Get<int>("value"), Is.EqualTo(10));
            Assert.That(definition.Storage.Get<int>("value"), Is.EqualTo(10));
        }

        [Test]
        public void CreateExecution_CopiesStepParametersAndInitializesState()
        {
            var definition = new SequenceDefinition();
            definition.AddStep<MockStepDefinition>().Delay = 0.25f;
            var second = definition.AddStep<MockStepDefinition>();
            second.Delay = 1.5f;
            second.InitOnStart = true;
            second.RunWithPreviousStep = true;
            var plan = Plan(definition);
            var execution = Execute(plan);
            Assert.That(execution.Duration, Is.EqualTo(plan.Duration));
            Assert.That(execution.CurrentTime, Is.Zero);
            Assert.That(execution.Steps, Has.Count.EqualTo(2));
            for (var i = 0; i < 2; i++)
            {
                var actual = execution.Steps[i];
                var expected = plan.Steps[i];
                Assert.That(actual.Definition, Is.SameAs(expected.Definition));
                Assert.That(actual.Player, Is.TypeOf<MockStepPlayer>());
                Assert.That(actual.StartTime, Is.EqualTo(expected.StartTime));
                Assert.That(actual.EndTime, Is.EqualTo(expected.EndTime));
                Assert.That(actual.Duration, Is.EqualTo(expected.Duration));
                Assert.That(actual.Delay, Is.EqualTo(expected.Delay));
                Assert.That(actual.InitOnStart, Is.EqualTo(expected.InitOnStart));
                Assert.That(actual.RunWithPreviousStep, Is.EqualTo(expected.RunWithPreviousStep));
                Assert.That(actual.ValuesApplied, Is.False);
                AssertProgress(actual, -1f);
            }
            Assert.That(execution.Steps[0].Player, Is.Not.SameAs(execution.Steps[1].Player));
        }

        [Test]
        public void CreateExecution_UnregisteredPlayer_ReportsAndOmitsOnlyBrokenStep()
        {
            var definition = new SequenceDefinition();
            var first = definition.AddStep<MockStepDefinition>();
            var broken = definition.AddStep<UnregisteredStepDefinition>();
            var last = definition.AddStep<MockStepDefinition>();
            var plan = Plan(definition);
            SequenceExecution execution = null;
            Assert.DoesNotThrow(() => execution = Execute(plan));
            Assert.That(execution.Steps.ConvertAll(s => s.Definition), Is.EqualTo(new[] { first, last }));
            Assert.That(plan.Steps, Has.Count.EqualTo(3));
            Assert.That(execution.Steps[1].StartTime, Is.EqualTo(4f));
            Assert.That(execution.Duration, Is.EqualTo(plan.Duration));
            Assert.That(Errors, Has.Count.EqualTo(1));
            Assert.That(Errors[0].Exception, Is.TypeOf<ArgumentException>());
            AssertError(SequentioStepErrorSource.CreatePlayer, broken, null, Errors[0].Exception);
        }
    }
}
