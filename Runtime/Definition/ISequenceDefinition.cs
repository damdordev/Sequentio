using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public interface ISequenceDefinition : IVarioStorageSource
    {
        IReadOnlyList<StepDefinition> Steps { get; }
    }
}