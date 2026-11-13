using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio.Tests
{
    // Run the same collection contract against all three requested definition owners.
    [TestFixture(typeof(SequenceDefinition))]
    [TestFixture(typeof(SequenceAsset))]
    [TestFixture(typeof(SequencePlayer))]
    public class SequenceDefinitionTests<T>
    {
        private ISequenceDefinition subject;
        private SequenceDefinition backingDefinition;
        private Object unityOwner;

        [SetUp]
        public void SetUp()
        {
            backingDefinition = new SequenceDefinition();
            if (typeof(T) == typeof(SequenceDefinition)) subject = backingDefinition;
            else if (typeof(T) == typeof(SequenceAsset))
            {
                var asset = ScriptableObject.CreateInstance<SequenceAsset>();
                unityOwner = asset;
                subject = asset;
            }
            else
            {
                // Keep inactive so serialized fields can be supplied before lifecycle callbacks.
                var go = new GameObject("SequencePlayer definition test");
                go.SetActive(false);
                unityOwner = go;
                var player = go.AddComponent<SequencePlayer>();
                subject = player;
                SetField(player, "settings", new SequenceSettings());
            }

            if (typeof(T) != typeof(SequenceDefinition)) SetField(subject, "definition", backingDefinition);
        }

        private static void SetField(object target, string name, object value)
        {
            var field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, "Serialized backing field " + name);
            field.SetValue(target, value);
        }

        [TearDown]
        public void TearDown()
        {
            if (unityOwner != null) Object.DestroyImmediate(unityOwner);
            // Deliberately do not release SequenceDefinition to its pool.
        }

        [Test]
        public void AddStep_AppendsDistinctResetStepsInOrder()
        {
            var first = subject.AddStep<MockStepDefinition>();
            var second = subject.AddStep<MockStepDefinition>();
            Assert.That(subject.Steps, Is.EqualTo(new[] { first, second }));
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first.ResetCalls, Is.EqualTo(1));
            Assert.That(second.ResetCalls, Is.EqualTo(1));
            Assert.That(first.Enabled.Evaluate(subject.Storage), Is.True);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void InsertStep_InsertsAtBeginningMiddleOrEnd(int index)
        {
            var first = subject.AddStep<MockStepDefinition>();
            var last = subject.AddStep<MockStepDefinition>();
            var inserted = subject.InsertStep<MockStepDefinition>(index);
            var expected = new System.Collections.Generic.List<StepDefinition> { first, last };
            expected.Insert(index, inserted);
            Assert.That(subject.Steps, Is.EqualTo(expected));
            Assert.That(inserted.ResetCalls, Is.EqualTo(1));
            Assert.That(inserted.Enabled.Evaluate(subject.Storage), Is.True);
        }

        [Test]
        public void InsertStep_IntoEmptyDefinition_AddsFirstStep()
        {
            var step = subject.InsertStep<MockStepDefinition>(0);
            Assert.That(subject.Steps, Is.EqualTo(new[] { step }));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void RemoveStep_RemovesOnlySelectedStepAndPreservesOrder(int index)
        {
            var expected = new System.Collections.Generic.List<StepDefinition>
            {
                subject.AddStep<MockStepDefinition>(), subject.AddStep<MockStepDefinition>(),
                subject.AddStep<MockStepDefinition>()
            };
            subject.RemoveStep(index);
            expected.RemoveAt(index);
            Assert.That(subject.Steps, Is.EqualTo(expected));
        }

        [Test]
        public void RemoveStep_OnlyStep_LeavesEmptyDefinition()
        {
            subject.AddStep<MockStepDefinition>();
            subject.RemoveStep(0);
            Assert.That(subject.Steps, Is.Empty);
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void InsertStep_InvalidIndex_ThrowsWithoutChangingSteps(int index)
        {
            var original = subject.AddStep<MockStepDefinition>();
            Assert.Throws<ArgumentOutOfRangeException>(() => subject.InsertStep<MockStepDefinition>(index));
            Assert.That(subject.Steps, Is.EqualTo(new[] { original }));
        }

        [TestCase(-1)]
        [TestCase(1)]
        public void RemoveStep_InvalidIndex_ThrowsWithoutChangingSteps(int index)
        {
            var original = subject.AddStep<MockStepDefinition>();
            Assert.Throws<ArgumentOutOfRangeException>(() => subject.RemoveStep(index));
            Assert.That(subject.Steps, Is.EqualTo(new[] { original }));
        }

        [Test]
        public void Reset_ClearsStepsAndStorageAndAllowsReuse()
        {
            subject.AddStep<MockStepDefinition>();
            subject.AddStep<MockStepDefinition>();
            subject.Storage.Update("number", 42);
            subject.Storage.Update("text", "test");
            var storage = subject.Storage;
            backingDefinition.Reset();
            Assert.That(subject.Steps, Is.Empty);
            Assert.That(subject.Storage, Is.SameAs(storage));
            Assert.That(subject.Storage.Variables, Is.Empty);
            Assert.That(subject.Storage.Contains("number"), Is.False);
            backingDefinition.Reset();
            var step = subject.AddStep<MockStepDefinition>();
            subject.Storage.Update("number", 7);
            Assert.That(subject.Steps, Is.EqualTo(new[] { step }));
            Assert.That(subject.Storage.Get<int>("number"), Is.EqualTo(7));
        }
    }
}
