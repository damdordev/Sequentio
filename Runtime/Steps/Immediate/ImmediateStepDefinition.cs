using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class ImmediateStepDefinition : StepDefinition
    {
        public override float CalculateDuration(VarioStorage storage) => 0f;
    }
}