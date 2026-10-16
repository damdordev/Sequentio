using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public interface ISequenceDefinition
    {
        SequenceSettings Settings { get; }
        IReadOnlyList<SequenceStep> Steps { get; }
        VarioStorage Storage { get; }
    }
}