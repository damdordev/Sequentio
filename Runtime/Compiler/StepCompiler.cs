// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System;

namespace Damdor.Sequentio
{

    /// <summary>
    /// Creates and configures pooled step players from step definitions.
    /// </summary>
    internal abstract class StepCompiler
    {
        /// <summary>
        /// Creates a pooled player and configures it with the supplied step definition.
        /// </summary>
        /// <param name="definition">The step definition to compile; it must match the compiler definition type.</param>
        /// <returns>A pooled player configured with the supplied definition.</returns>
        /// <exception cref="System.ArgumentException">The definition does not match the compiler definition type.</exception>
        /// <exception cref="System.InvalidOperationException">A player could not be obtained from the pool.</exception>
        public abstract StepPlayer Compile(StepDefinition definition);
    }

    /// <summary>
    /// Creates and configures pooled step players from step definitions.
    /// </summary>
    /// <typeparam name="TStepDefinition">The step definition type handled by the player or compiler.</typeparam>
    /// <typeparam name="TStepPlayer">The concrete step player type, which must have a public parameterless constructor.</typeparam>
    internal class StepCompiler<TStepDefinition, TStepPlayer> : StepCompiler
        where TStepDefinition : StepDefinition
        where TStepPlayer : StepPlayer<TStepDefinition>, new()
    {

        /// <inheritdoc />
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