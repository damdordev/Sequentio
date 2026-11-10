using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public interface ISequenceDefinition
    {
        IReadOnlyList<StepDefinition> Steps { get; }
        VarioStorage Storage { get; }
        SequencePlan CreateDefinitionPlan(SequenceSettings settings);
    }
}