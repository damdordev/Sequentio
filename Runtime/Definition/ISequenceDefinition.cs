using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public interface ISequenceDefinition
    {
        SequenceSettings Settings { get; }
        IReadOnlyList<StepDefinition> Steps { get; }
        VarioStorage Storage { get; }
        SequencePlan CreateDefinitionPlan();
    }
}