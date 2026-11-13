using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Damdor.Sequentio.Tests
{
    public class SequenceExecutorTests : SequenceTestBase
    {
        private SequenceExecution Build(SequenceSettings settings = null, float duration = 2f,
            float delay = 0f, bool initOnStart = false, int count = 1, bool concurrent = false)
        {
            var definition = new SequenceDefinition();
            for (var i = 0; i < count; i++)
            {
                var step = definition.AddStep<MockStepDefinition>();
                step.Duration = duration;
                step.Delay = delay;
                step.InitOnStart = initOnStart;
                step.RunWithPreviousStep = concurrent && i > 0;
            }
            return Execute(Plan(definition, settings));
        }

        private SequenceExecution BuildSharedTarget(MockStepTarget target, List<string> callOrder, int count = 3,
            bool initOnStart = false)
        {
            var definition = new SequenceDefinition();
            var labels = new[] { "first", "second", "third" };
            for (var i = 0; i < count; i++)
            {
                var step = definition.AddStep<MockStepDefinition>();
                step.Duration = 1f;
                step.InitOnStart = initOnStart;
                step.Target = target;
                step.TargetValue = (i + 1) * 10f;
                step.Label = labels[i];
                step.CallOrder = callOrder;
            }
            return Execute(Plan(definition));
        }

        private static void AssertStepState(StepExecution step, float progress, bool initialized)
        {
            AssertProgress(step, progress);
            Assert.That(step.ValuesApplied, Is.EqualTo(initialized));
            Assert.That(((MockStepPlayer)step.Player).InitCalls, Is.EqualTo(initialized ? 1 : 0));
        }

        [Test]
        public void ReturnToInitialState_SeekingToSameTimeAgain_ReappliesTargetValue(
            [Values(false, true)] bool initOnStart, [Values(0.5f, 1f)] float time)
        {
            var target = new MockStepTarget { Value = 2f };
            var execution = BuildSharedTarget(target, new List<string>(), count: 1, initOnStart: initOnStart);
            var executor = Executor(execution);
            var step = execution.Steps[0];
            var player = (MockStepPlayer)step.Player;

            executor.SetTime(time);
            AssertStepState(step, time, true);
            Assert.That(target.Value, Is.EqualTo(2f + 8f * time));
            var progressCalls = player.ProgressCalls.Count;

            executor.ReturnToInitialState();
            Assert.That(target.Value, Is.EqualTo(2f));
            Assert.That(execution.CurrentTime, Is.Zero);
            Assert.That(player.ReturnCalls, Is.EqualTo(1));

            executor.SetTime(time);
            AssertStepState(step, time, true);
            Assert.That(player.ProgressCalls, Has.Count.EqualTo(progressCalls + 1));
            Assert.That(target.Value, Is.EqualTo(2f + 8f * time), "Target must be reapplied after restoration");
            Assert.That(Errors, Is.Empty);
        }

        [Test]
        public void Update_ReverseMultipleSteps_CrossesDifferentDurationsAndDelays(
            [Values(false, true)] bool initOnStart, [Values(false, true)] bool irregular)
        {
            var definition = new SequenceDefinition();
            var first = definition.AddStep<MockStepDefinition>();
            first.Duration = 1f;
            first.Delay = 0.5f; // [0.5, 1.5]
            var second = definition.AddStep<MockStepDefinition>();
            second.Duration = 2f;
            second.Delay = 0.5f; // [2, 4]
            var third = definition.AddStep<MockStepDefinition>();
            third.Duration = 1f;
            third.Delay = 1f; // [5, 6]
            foreach (var step in definition.Steps) step.InitOnStart = initOnStart;
            var execution = Execute(Plan(definition, new SequenceSettings { Direction = PlayDirection.Reverse }));
            var executor = Executor(execution);

            Assert.That(execution.CurrentTime, Is.EqualTo(6f));
            foreach (var step in execution.Steps)
                AssertStepState(step, initOnStart ? 0f : -1f, initOnStart);
            Assert.That(executor.Update(0f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            foreach (var step in execution.Steps) AssertStepState(step, 1f, true);

            var deltas = irregular
                ? new[] { 0.25f, 0.75f, 1.5f, 1.75f, 10f }
                : new[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
            var times = irregular
                ? new[] { 5.75f, 5f, 3.5f, 1.75f, 0f }
                : new[] { 5.5f, 5f, 4.5f, 4f, 3.5f, 3f, 2.5f, 2f, 1.5f, 1f, 0.5f, 0f };
            var firstProgress = irregular
                ? new[] { 1f, 1f, 1f, 1f, 0f }
                : new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0f, 0f };
            var secondProgress = irregular
                ? new[] { 1f, 1f, 0.75f, 0f, 0f }
                : new[] { 1f, 1f, 1f, 1f, 0.75f, 0.5f, 0.25f, 0f, 0f, 0f, 0f, 0f };
            var thirdProgress = irregular
                ? new[] { 0.75f, 0f, 0f, 0f, 0f }
                : new[] { 0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            for (var i = 0; i < deltas.Length; i++)
            {
                var result = executor.Update(deltas[i]);
                Assert.That(execution.CurrentTime, Is.EqualTo(times[i]));
                AssertStepState(execution.Steps[0], firstProgress[i], true);
                AssertStepState(execution.Steps[1], secondProgress[i], true);
                AssertStepState(execution.Steps[2], thirdProgress[i], true);
                Assert.That(result, Is.EqualTo(i == deltas.Length - 1
                    ? SequenceExecutorUpdateResult.Finished : SequenceExecutorUpdateResult.NotFinished));
            }
            Assert.That(Errors, Is.Empty);
        }

        [Test]
        public void Update_MultipleStepsLooping_RepeatedBoundariesPreserveProgressAndInitialization(
            [Values(LoopMode.Loop, LoopMode.PingPong)] LoopMode loop,
            [Values] PlayDirection direction, [Values(false, true)] bool stutter,
            [Values(false, true)] bool initOnStart)
        {
            var definition = new SequenceDefinition();
            definition.AddStep<MockStepDefinition>().Duration = 2f; // [0, 2]
            var concurrent = definition.AddStep<MockStepDefinition>();
            concurrent.Duration = 1f;
            concurrent.Delay = 1f;
            concurrent.RunWithPreviousStep = true; // [1, 2]
            var following = definition.AddStep<MockStepDefinition>();
            following.Duration = 1f;
            following.Delay = 1f; // [3, 4]
            foreach (var step in definition.Steps) step.InitOnStart = initOnStart;
            var execution = Execute(Plan(definition, new SequenceSettings { Direction = direction, Loop = loop }));
            var executor = Executor(execution);
            foreach (var step in execution.Steps)
                AssertStepState(step, initOnStart ? 0f : -1f, initOnStart);

            float[] times;
            if (stutter)
            {
                times = loop == LoopMode.Loop
                    ? direction == PlayDirection.Forward
                        ? new[] { 1f, 2f, 1f, 2f, 0f, 1f }
                        : new[] { 3f, 2f, 3f, 2f, 4f, 3f }
                    : direction == PlayDirection.Forward
                        ? new[] { 1f, 2f, 1f, 2f, 4f, 3f }
                        : new[] { 3f, 2f, 3f, 2f, 0f, 1f };
            }
            else
            {
                times = loop == LoopMode.Loop
                    ? direction == PlayDirection.Forward
                        ? new[] { 1f, 2f, 3f, 4f, 1f, 2f, 3f, 4f, 1f, 2f, 3f, 4f }
                        : new[] { 3f, 2f, 1f, 0f, 3f, 2f, 1f, 0f, 3f, 2f, 1f, 0f }
                    : direction == PlayDirection.Forward
                        ? new[] { 1f, 2f, 3f, 4f, 3f, 2f, 1f, 0f, 1f, 2f, 3f, 4f }
                        : new[] { 3f, 2f, 1f, 0f, 1f, 2f, 3f, 4f, 3f, 2f, 1f, 0f };
            }
            var stutterDeltas = new[] { 9f, 1f, 7f, 1f, 10f, 1f };
            for (var i = 0; i < times.Length; i++)
            {
                Assert.That(executor.Update(stutter ? stutterDeltas[i] : 1f),
                    Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
                Assert.That(execution.CurrentTime, Is.EqualTo(times[i]), "Update " + i);
                var thirdInitialized = initOnStart || stutter || direction == PlayDirection.Reverse || i >= 2;
                AssertStepState(execution.Steps[0], Math.Min(times[i] / 2f, 1f), true);
                AssertStepState(execution.Steps[1], Math.Max(0f, Math.Min(times[i] - 1f, 1f)), true);
                AssertStepState(execution.Steps[2], thirdInitialized
                    ? Math.Max(0f, Math.Min(times[i] - 3f, 1f)) : -1f, thirdInitialized);
            }
            Assert.That(Errors, Is.Empty);
        }

        [Test]
        public void SetTime_SharedTarget_AppliesEarlierStepsBeforeInitializingLaterSteps(
            [Values(false, true)] bool viaUpdate)
        {
            var target = new MockStepTarget();
            var calls = new List<string>();
            var execution = BuildSharedTarget(target, calls);
            var executor = Executor(execution);
            if (viaUpdate) executor.Update(2.5f);
            else executor.SetTime(2.5f);

            Assert.That(calls, Is.EqualTo(new[]
            {
                "first.Init", "first.Progress", "second.Init", "second.Progress", "third.Init", "third.Progress"
            }));
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).InitialValue, Is.Zero);
            Assert.That(((MockStepPlayer)execution.Steps[1].Player).InitialValue, Is.EqualTo(10f));
            Assert.That(((MockStepPlayer)execution.Steps[2].Player).InitialValue, Is.EqualTo(20f));
            AssertStepState(execution.Steps[0], 1f, true);
            AssertStepState(execution.Steps[1], 1f, true);
            AssertStepState(execution.Steps[2], 0.5f, true);
            Assert.That(target.Value, Is.EqualTo(25f));
        }

        [Test]
        public void SetTime_SharedTarget_RewindsLaterStepsBeforeApplyingEarlierStep()
        {
            var target = new MockStepTarget();
            var calls = new List<string>();
            var execution = BuildSharedTarget(target, calls);
            var executor = Executor(execution);
            executor.SetTime(3f);
            Assert.That(target.Value, Is.EqualTo(30f));
            calls.Clear();

            executor.SetTime(0.5f);
            Assert.That(calls, Is.EqualTo(new[] { "third.Progress", "second.Progress", "first.Progress" }));
            AssertStepState(execution.Steps[0], 0.5f, true);
            AssertStepState(execution.Steps[1], 0f, true);
            AssertStepState(execution.Steps[2], 0f, true);
            Assert.That(target.Value, Is.EqualTo(5f));

            calls.Clear();
            executor.SetTime(2.5f);
            Assert.That(calls, Is.EqualTo(new[] { "first.Progress", "second.Progress", "third.Progress" }));
            AssertStepState(execution.Steps[0], 1f, true);
            AssertStepState(execution.Steps[1], 1f, true);
            AssertStepState(execution.Steps[2], 0.5f, true);
            Assert.That(target.Value, Is.EqualTo(25f));
        }

        [Test]
        public void ReturnToInitialState_SharedTarget_RestoresOriginalValueInReverseOrder()
        {
            var target = new MockStepTarget { Value = 2f };
            var calls = new List<string>();
            var execution = BuildSharedTarget(target, calls);
            var executor = Executor(execution);
            executor.SetTime(2.5f);
            Assert.That(target.Value, Is.EqualTo(25f));
            calls.Clear();

            executor.ReturnToInitialState();
            Assert.That(calls, Is.EqualTo(new[] { "third.Return", "second.Return", "first.Return" }));
            Assert.That(target.Value, Is.EqualTo(2f));
            Assert.That(execution.CurrentTime, Is.Zero);
            foreach (var step in execution.Steps)
            {
                AssertStepState(step, 0f, true);
                Assert.That(((MockStepPlayer)step.Player).ReturnCalls, Is.EqualTo(1));
            }
        }

        [Test]
        public void Update_RegularIntervals_AdvancesBothConsecutiveSteps()
        {
            var execution = Build(count: 2);
            var executor = Executor(execution);
            for (var i = 1; i <= 8; i++)
            {
                var result = executor.Update(0.5f);
                var time = i * 0.5f;
                Assert.That(execution.CurrentTime, Is.EqualTo(time));
                AssertProgress(execution.Steps[0], Math.Min(time / 2f, 1f));
                AssertProgress(execution.Steps[1], time < 2f ? -1f : (time - 2f) / 2f);
                Assert.That(result, Is.EqualTo(i == 8 ? SequenceExecutorUpdateResult.Finished : SequenceExecutorUpdateResult.NotFinished));
            }
        }

        [Test]
        public void Update_IrregularIntervalsAndStutter_CompletesSkippedSteps()
        {
            var execution = Build(count: 3);
            var executor = Executor(execution);
            Assert.That(executor.Update(0.1f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            AssertProgress(execution.Steps[0], 0.05f);
            executor.Update(0.65f);
            AssertProgress(execution.Steps[0], 0.375f);
            executor.Update(4f);
            Assert.That(execution.CurrentTime, Is.EqualTo(4.75f));
            AssertProgress(execution.Steps[0], 1f);
            AssertProgress(execution.Steps[1], 1f);
            AssertProgress(execution.Steps[2], 0.375f);
            Assert.That(executor.Update(100f), Is.EqualTo(SequenceExecutorUpdateResult.Finished));
            Assert.That(execution.CurrentTime, Is.EqualTo(6f));
            foreach (var step in execution.Steps)
            {
                AssertProgress(step, 1f);
                Assert.That(step.ValuesApplied, Is.True);
                Assert.That(((MockStepPlayer)step.Player).InitCalls, Is.EqualTo(1));
            }
        }

        [TestCase(0f, 0f)]
        [TestCase(0.5f, 0.25f)]
        [TestCase(1f, 0.5f)]
        [TestCase(2f, 1f)]
        [TestCase(-1f, 0f)]
        public void Update_AppliesSpeed(float speed, float expectedTime)
        {
            var execution = Build(new SequenceSettings { Speed = speed });
            var executor = Executor(execution);
            Assert.That(executor.Update(0.5f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.EqualTo(expectedTime));
            AssertProgress(execution.Steps[0], expectedTime / 2f);
        }

        [Test]
        public void Update_ChangingSpeedDuringPlayback_TakesEffectOnNextUpdate()
        {
            var execution = Build();
            var executor = Executor(execution);
            executor.Update(0.25f);
            execution.Settings.Speed = 2f;
            executor.Update(0.25f);
            Assert.That(execution.CurrentTime, Is.EqualTo(0.75f));
            AssertProgress(execution.Steps[0], 0.375f);
        }

        [Test]
        public void Update_ZeroDelta_DoesNotAdvanceOrRepeatProgressCallbacks()
        {
            var execution = Build();
            var executor = Executor(execution);
            executor.Update(0f);
            executor.Update(0f);
            Assert.That(execution.CurrentTime, Is.Zero);
            AssertProgress(execution.Steps[0], 0f);
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).ProgressCalls, Is.EqualTo(new[] { 0f }));
        }

        [TestCase(PlayDirection.Forward, 0f, 0.5f, 0.25f)]
        [TestCase(PlayDirection.Reverse, 2f, 1.5f, 0.75f)]
        public void Update_UsesInitialDirection(PlayDirection direction, float initialTime, float time, float progress)
        {
            var execution = Build(new SequenceSettings { Direction = direction });
            var executor = Executor(execution);
            Assert.That(execution.CurrentTime, Is.EqualTo(initialTime));
            executor.Update(0.5f);
            Assert.That(execution.CurrentTime, Is.EqualTo(time));
            AssertProgress(execution.Steps[0], progress);
        }

        [Test]
        public void Update_OtherSettings_DoNotAlterExplicitDelta(
            [Values] AutoPlayMode autoPlay, [Values] TimescaleMode timescale, [Values] StopBehaviour stop)
        {
            var execution = Build(new SequenceSettings
            {
                AutoPlay = autoPlay, Timescale = timescale, DefaultStopBehaviour = stop
            });
            var executor = Executor(execution);
            Assert.That(executor.Update(0.5f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.EqualTo(0.5f));
            AssertProgress(execution.Steps[0], 0.25f);
        }

        [Test]
        public void Update_NoLoop_ExactBoundaryAndOvershootFinish(
            [Values] PlayDirection direction, [Values(2f, 200f)] float dt)
        {
            var execution = Build(new SequenceSettings { Direction = direction });
            var executor = Executor(execution);
            Assert.That(executor.Update(dt), Is.EqualTo(SequenceExecutorUpdateResult.Finished));
            var expected = direction == PlayDirection.Forward ? 1f : 0f;
            Assert.That(execution.CurrentTime, Is.EqualTo(expected * 2f));
            AssertProgress(execution.Steps[0], expected);
            Assert.That(executor.Update(1f), Is.EqualTo(SequenceExecutorUpdateResult.Finished));
            AssertProgress(execution.Steps[0], expected);
        }

        [TestCase(PlayDirection.Forward, 2.5f, 0.5f)]
        [TestCase(PlayDirection.Forward, 6.5f, 0.5f)]
        [TestCase(PlayDirection.Reverse, 2.5f, 1.5f)]
        [TestCase(PlayDirection.Reverse, 6.5f, 1.5f)]
        public void Update_Loop_WrapsSingleAndMultipleCycles(PlayDirection direction, float dt, float time)
        {
            var execution = Build(new SequenceSettings { Direction = direction, Loop = LoopMode.Loop });
            var executor = Executor(execution);
            Assert.That(executor.Update(dt), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.EqualTo(time));
            AssertProgress(execution.Steps[0], time / 2f);
            var player = (MockStepPlayer)execution.Steps[0].Player;
            Assert.That(player.ProgressCalls[0], Is.EqualTo(direction == PlayDirection.Forward ? 1f : 0f));
            executor.Update(0.25f);
            AssertProgress(execution.Steps[0], (time + (direction == PlayDirection.Forward ? 0.25f : -0.25f)) / 2f);
            Assert.That(player.InitCalls, Is.EqualTo(1));
        }

        [TestCase(PlayDirection.Forward, 2.5f, 1.5f, 1.25f)]
        [TestCase(PlayDirection.Forward, 4.5f, 0.5f, 0.75f)]
        [TestCase(PlayDirection.Forward, 6.5f, 1.5f, 1.25f)]
        [TestCase(PlayDirection.Reverse, 2.5f, 0.5f, 0.75f)]
        [TestCase(PlayDirection.Reverse, 4.5f, 1.5f, 1.25f)]
        [TestCase(PlayDirection.Reverse, 6.5f, 0.5f, 0.75f)]
        public void Update_PingPong_ReflectsTimeAndPreservesCorrectDirectionAfterStutter(
            PlayDirection direction, float dt, float time, float nextTime)
        {
            var execution = Build(new SequenceSettings { Direction = direction, Loop = LoopMode.PingPong });
            var executor = Executor(execution);
            Assert.That(executor.Update(dt), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.EqualTo(time));
            AssertProgress(execution.Steps[0], time / 2f);
            Assert.That(executor.Update(0.25f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.EqualTo(nextTime));
            AssertProgress(execution.Steps[0], nextTime / 2f);
        }

        [Test]
        public void Update_Looping_ExactBoundaryContinuesAndNextUpdateCrossesBoundary(
            [Values(LoopMode.Loop, LoopMode.PingPong)] LoopMode loop, [Values] PlayDirection direction)
        {
            var execution = Build(new SequenceSettings { Direction = direction, Loop = loop });
            var executor = Executor(execution);
            Assert.That(executor.Update(2f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            AssertProgress(execution.Steps[0], direction == PlayDirection.Forward ? 1f : 0f);
            Assert.That(executor.Update(0.5f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            var time = loop == LoopMode.PingPong
                ? direction == PlayDirection.Forward ? 1.5f : 0.5f
                : direction == PlayDirection.Forward ? 0.5f : 1.5f;
            Assert.That(execution.CurrentTime, Is.EqualTo(time));
            AssertProgress(execution.Steps[0], time / 2f);
        }

        [Test]
        public void Update_Looping_ExactMultipleOfDuration_PreservesNextDirection(
            [Values(LoopMode.Loop, LoopMode.PingPong)] LoopMode loop, [Values] PlayDirection direction)
        {
            var execution = Build(new SequenceSettings { Loop = loop, Direction = direction });
            var executor = Executor(execution);
            Assert.That(executor.Update(4f), Is.EqualTo(SequenceExecutorUpdateResult.NotFinished));
            var expectedTime = direction == PlayDirection.Forward ? 0f : 2f;
            Assert.That(execution.CurrentTime, Is.EqualTo(expectedTime));
            AssertProgress(execution.Steps[0], expectedTime / 2f);
            executor.Update(0.5f);
            expectedTime = direction == PlayDirection.Forward ? 0.5f : 1.5f;
            Assert.That(execution.CurrentTime, Is.EqualTo(expectedTime));
            AssertProgress(execution.Steps[0], expectedTime / 2f);
        }

        [Test]
        public void Update_EmptySequence_HandlesZeroDuration(
            [Values] LoopMode loop, [Values] PlayDirection direction)
        {
            var execution = Execute(Plan(new SequenceDefinition(), new SequenceSettings { Loop = loop, Direction = direction }));
            var executor = Executor(execution);
            Assert.That(executor.Update(100f), Is.EqualTo(loop == LoopMode.None
                ? SequenceExecutorUpdateResult.Finished : SequenceExecutorUpdateResult.NotFinished));
            Assert.That(execution.CurrentTime, Is.Zero);
            Assert.That(execution.Steps, Is.Empty);
        }

        [Test]
        public void Update_ZeroDurationStep_CompletesWithoutDivisionByZero([Values] LoopMode loop)
        {
            var execution = Build(new SequenceSettings { Loop = loop }, duration: 0f);
            var executor = Executor(execution);
            var result = executor.Update(10f);
            Assert.That(result, Is.EqualTo(loop == LoopMode.None ? SequenceExecutorUpdateResult.Finished : SequenceExecutorUpdateResult.NotFinished));
            AssertProgress(execution.Steps[0], 1f);
        }

        [Test]
        public void SetTime_DelayedZeroDurationStep_CompletesExactlyAtStart()
        {
            var execution = Build(duration: 0f, delay: 1f);
            var executor = Executor(execution);
            executor.SetTime(0.5f);
            Assert.That(execution.Steps[0].ValuesApplied, Is.False);
            AssertProgress(execution.Steps[0], -1f);
            executor.SetTime(1f);
            Assert.That(execution.Steps[0].ValuesApplied, Is.True);
            AssertProgress(execution.Steps[0], 1f);
            executor.SetTime(0f);
            AssertProgress(execution.Steps[0], 0f);
            executor.SetTime(1f);
            AssertProgress(execution.Steps[0], 1f);
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).InitCalls, Is.EqualTo(1));
        }

        [Test]
        public void InitOnStart_InitializesImmediatelyOrAtScheduledStart([Values(false, true)] bool initOnStart)
        {
            var execution = Build(delay: 1f, initOnStart: initOnStart);
            var step = execution.Steps[0];
            var player = (MockStepPlayer)step.Player;
            var executor = Executor(execution);
            Assert.That(step.ValuesApplied, Is.EqualTo(initOnStart));
            Assert.That(player.InitCalls, Is.EqualTo(initOnStart ? 1 : 0));
            AssertProgress(step, initOnStart ? 0f : -1f);
            executor.Update(0.5f);
            Assert.That(step.ValuesApplied, Is.EqualTo(initOnStart));
            Assert.That(player.InitCalls, Is.EqualTo(initOnStart ? 1 : 0));
            AssertProgress(step, initOnStart ? 0f : -1f);
            executor.Update(0.5f);
            Assert.That(step.ValuesApplied, Is.True);
            Assert.That(player.InitCalls, Is.EqualTo(1));
            Assert.That(player.InitializedStorage, Is.SameAs(execution.Storage));
            Assert.That(player.InitializedDefinition, Is.SameAs(step.Definition));
            AssertProgress(step, 0f);
            executor.Update(1f);
            AssertProgress(step, 0.5f);
            Assert.That(player.InitCalls, Is.EqualTo(1));
        }

        [Test]
        public void SetTime_RepeatedForwardAndBackwardSeeks_ClampsAndUpdatesAllSteps()
        {
            var execution = Build(count: 2);
            var executor = Executor(execution);
            var times = new[] { -5f, 1f, 2f, 3f, 20f, 0.5f, 0.5f, 0f, 3f };
            var first = new[] { 0f, 0.5f, 1f, 1f, 1f, 0.25f, 0.25f, 0f, 1f };
            var second = new[] { -1f, -1f, 0f, 0.5f, 1f, 0f, 0f, 0f, 0.5f };
            for (var i = 0; i < times.Length; i++)
            {
                executor.SetTime(times[i]);
                Assert.That(execution.CurrentTime, Is.EqualTo(Math.Max(0f, Math.Min(4f, times[i]))));
                AssertProgress(execution.Steps[0], first[i]);
                AssertProgress(execution.Steps[1], second[i]);
                Assert.That(execution.Steps[0].ValuesApplied, Is.True);
                Assert.That(execution.Steps[1].ValuesApplied, Is.EqualTo(i >= 2));
            }
            foreach (var step in execution.Steps)
                Assert.That(((MockStepPlayer)step.Player).InitCalls, Is.EqualTo(1));
        }

        [Test]
        public void SetTime_ConcurrentStepsWithDifferentDelays_ApplyIndependentProgress()
        {
            var definition = new SequenceDefinition();
            definition.AddStep<MockStepDefinition>().Duration = 4f;
            var second = definition.AddStep<MockStepDefinition>();
            second.RunWithPreviousStep = true;
            second.Delay = 1f;
            var execution = Execute(Plan(definition));
            var executor = Executor(execution);
            executor.SetTime(2f);
            AssertProgress(execution.Steps[0], 0.5f);
            AssertProgress(execution.Steps[1], 0.5f);
            executor.SetTime(0.5f);
            AssertProgress(execution.Steps[0], 0.125f);
            AssertProgress(execution.Steps[1], 0f);
        }

        [Test]
        public void SetTime_UnchangedProgress_DoesNotCallPlayerAgain()
        {
            var execution = Build();
            var executor = Executor(execution);
            executor.SetTime(1f);
            executor.SetTime(1f);
            AssertProgress(execution.Steps[0], 0.5f);
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).ProgressCalls, Has.Count.EqualTo(1));
        }

        [Test]
        public void ReturnToInitialState_RestoresInitializedStepsInReverseOrder()
        {
            var execution = Build(count: 3);
            var executor = Executor(execution);
            var returned = new List<int>();
            for (var i = 0; i < 3; i++)
            {
                var index = i;
                ((MockStepPlayer)execution.Steps[i].Player).OnReturn = () => returned.Add(index);
            }
            executor.SetTime(3f);
            AssertProgress(execution.Steps[0], 1f);
            AssertProgress(execution.Steps[1], 0.5f);
            AssertProgress(execution.Steps[2], -1f);
            executor.ReturnToInitialState();
            Assert.That(execution.CurrentTime, Is.Zero);
            Assert.That(returned, Is.EqualTo(new[] { 1, 0 }));
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).ReturnCalls, Is.EqualTo(1));
            Assert.That(((MockStepPlayer)execution.Steps[1].Player).ReturnCalls, Is.EqualTo(1));
            Assert.That(((MockStepPlayer)execution.Steps[2].Player).ReturnCalls, Is.Zero);
            AssertProgress(execution.Steps[0], 0f);
            AssertProgress(execution.Steps[1], 0f);
            AssertProgress(execution.Steps[2], -1f);
            executor.SetTime(3f);
            foreach (var step in execution.Steps.GetRange(0, 2))
                Assert.That(((MockStepPlayer)step.Player).InitCalls, Is.EqualTo(1));
        }

        [Test]
        public void ReturnToInitialState_BeforeInitialization_DoesNotTouchPlayers()
        {
            var execution = Build();
            var executor = Executor(execution);
            executor.ReturnToInitialState();
            AssertProgress(execution.Steps[0], -1f);
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).ReturnCalls, Is.Zero);
        }

        [Test]
        public void Init_OnlyOnceAcrossSeeksUpdatesAndLoops(
            [Values(false, true)] bool initOnStart, [Values] LoopMode loop)
        {
            var execution = Build(new SequenceSettings { Loop = loop }, initOnStart: initOnStart);
            var executor = Executor(execution);
            executor.SetTime(1f);
            executor.SetTime(0f);
            executor.Update(10f);
            executor.SetTime(1f);
            Assert.That(execution.Steps[0].ValuesApplied, Is.True);
            Assert.That(((MockStepPlayer)execution.Steps[0].Player).InitCalls, Is.EqualTo(1));
            AssertProgress(execution.Steps[0], 0.5f);
        }

        [Test]
        public void InitException_IsReportedAndDoesNotAffectOtherSteps([Values(false, true)] bool initOnStart)
        {
            var execution = Build(initOnStart: initOnStart, count: 3, concurrent: true);
            var broken = execution.Steps[1];
            var player = (MockStepPlayer)broken.Player;
            var exception = new InvalidOperationException("init failed");
            player.InitException = exception;
            SequenceExecutor executor = null;
            Assert.DoesNotThrow(() => executor = Executor(execution));
            Assert.DoesNotThrow(() => executor.SetTime(1f));
            foreach (var step in execution.Steps)
            {
                Assert.That(step.ValuesApplied, Is.True);
                Assert.That(((MockStepPlayer)step.Player).InitCalls, Is.EqualTo(1));
                AssertProgress(step, 0.5f);
            }
            AssertError(SequentioStepErrorSource.InitPlayer, broken.Definition, player, exception);
        }

        [TestCase(0)] // initialization on creation
        [TestCase(1)] // advancing via Update
        [TestCase(2)] // seeking before a previously initialized step
        public void SetProgressException_IsReportedAndDoesNotAffectOtherSteps(int operation)
        {
            var execution = Build(delay: 1f, initOnStart: operation == 0, count: 3, concurrent: true);
            var broken = execution.Steps[1];
            var player = (MockStepPlayer)broken.Player;
            var exception = new InvalidOperationException("progress failed");
            SequenceExecutor executor = null;
            if (operation != 0) executor = Executor(execution);
            if (operation == 2) executor.SetTime(2f);
            player.ProgressException = exception;
            if (operation == 0) Assert.DoesNotThrow(() => executor = Executor(execution));
            else if (operation == 1) Assert.DoesNotThrow(() => executor.Update(2f));
            else Assert.DoesNotThrow(() => executor.SetTime(0f));
            foreach (var step in execution.Steps) AssertProgress(step, operation == 1 ? 0.5f : 0f);
            AssertError(SequentioStepErrorSource.SetProgress, broken.Definition, player, exception);
        }

        [Test]
        public void ReturnException_IsReportedAndDoesNotAffectOtherSteps()
        {
            var execution = Build(count: 3, concurrent: true);
            var executor = Executor(execution);
            executor.SetTime(1f);
            var broken = execution.Steps[1];
            var player = (MockStepPlayer)broken.Player;
            var exception = new InvalidOperationException("return failed");
            player.ReturnException = exception;
            Assert.DoesNotThrow(executor.ReturnToInitialState);
            Assert.That(execution.CurrentTime, Is.Zero);
            foreach (var step in execution.Steps)
                Assert.That(((MockStepPlayer)step.Player).ReturnCalls, Is.EqualTo(1));
            AssertError(SequentioStepErrorSource.ReturnToInitialState, broken.Definition, player, exception);
        }
    }
}
