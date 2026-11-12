using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Describes an exception raised while compiling or playing a step.
    /// </summary>
    public readonly struct SequentioStepErrorHandlerData
    {
        /// <summary>
        /// The operation that raised the exception.
        /// </summary>
        public readonly SequentioStepErrorSource ErrorSource { get; }
        /// <summary>
        /// The step definition involved in the error.
        /// </summary>
        public readonly StepDefinition Definition { get; }
        /// <summary>
        /// The player that applies this step, or null when no player is available.
        /// </summary>
        public readonly StepPlayer Player { get; }
        /// <summary>
        /// The exception raised by the step operation.
        /// </summary>
        public readonly Exception Exception { get; }

        /// <summary>
        /// Creates an error report for a step operation.
        /// </summary>
        /// <param name="errorSource">The operation that raised the exception.</param>
        /// <param name="definition">The definition used to configure playback.</param>
        /// <param name="player">The player involved in the error, or null if no player was created.</param>
        /// <param name="exception">The exception raised by the operation.</param>
        public SequentioStepErrorHandlerData(SequentioStepErrorSource errorSource, StepDefinition definition, StepPlayer player, Exception exception)
        {
            ErrorSource = errorSource;
            Definition = definition;
            Player = player;
            Exception = exception;
        }
    }
}