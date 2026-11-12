using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Provides the ordered steps and variable storage that define a sequence.
    /// </summary>
    public interface ISequenceDefinition : IVarioStorageSource
    {
        /// <summary>
        /// The ordered steps in this sequence.
        /// </summary>
        IReadOnlyList<StepDefinition> Steps { get; }

        /// <summary>
        /// Creates a new step of the specified type, adds it to the sequence, and returns the added step.
        /// </summary>
        /// <typeparam name="T">The type of step definition to create and add.</typeparam>
        /// <return>The newly added step definition.</return>
        T AddStep<T>() where T : StepDefinition, new();

        /// <summary>
        /// Removes the step at the specified index from the sequence.
        /// </summary>
        /// <param name="index">The zero-based index of the step to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range.</exception>
        void RemoveStep(int index);

        /// <summary>
        /// Creates a new step of the specified type, inserts it into the sequence at the specified index, and returns the inserted step.
        /// </summary>
        /// <typeparam name="T">The type of step definition to create and insert.</typeparam>
        /// <param name="index">The zero-based index at which to insert the new step.</param>
        /// <return>The newly inserted step definition.</return>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range.</exception>
        T InsertStep<T>(int index) where T : StepDefinition, new();
    }
}