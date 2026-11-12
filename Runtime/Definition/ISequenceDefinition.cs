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
    }
}