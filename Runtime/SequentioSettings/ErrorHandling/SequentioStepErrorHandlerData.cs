using System;

namespace Damdor.Sequentio
{
    public readonly struct SequentioStepErrorHandlerData
    {
        public readonly SequentioStepErrorSource ErrorSource { get; }
        public readonly StepDefinition Definition { get; }
        public readonly StepPlayer Player { get; }
        public readonly Exception Exception { get; }

        public SequentioStepErrorHandlerData(SequentioStepErrorSource errorSource, StepDefinition definition, StepPlayer player, Exception exception)
        {
            ErrorSource = errorSource;
            Definition = definition;
            Player = player;
            Exception = exception;
        }
    }
}