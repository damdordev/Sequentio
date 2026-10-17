// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;

namespace Damdor.Sequentio
{

    internal abstract class StepCompiler
    {
        public abstract StepPlayer Compile(StepDefinition definition);
    }

    internal class StepCompiler<TStepDefinition, TStepPlayer> : StepCompiler
        where TStepDefinition : StepDefinition
        where TStepPlayer : StepPlayer<TStepDefinition>, new()
    {

        public override StepPlayer Compile(StepDefinition definition)
        {
            if (definition is not TStepDefinition typedDefinition)
            {
                throw new ArgumentException(
                    $"Invalid step definition type: {definition.GetType().FullName}, expected: {typeof(TStepDefinition).FullName}");
            }

            var player = SequentioPooling.PopStepPlayer<TStepPlayer>();

            if (player == null)
            {
                throw new InvalidOperationException(
                    $"Failed to create step player for type: {typeof(TStepPlayer).FullName}");
            }

            player.Setup(typedDefinition);
            return player;
        }

    }

}