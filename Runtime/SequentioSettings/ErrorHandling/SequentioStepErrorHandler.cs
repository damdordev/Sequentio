namespace Damdor.Sequentio
{
    /// <summary>
    /// Handles an exception raised while compiling or playing a Sequentio step.
    /// </summary>
    /// <param name="errorData">The step error context, including its source and exception.</param>
    public delegate void SequentioStepErrorHandler(SequentioStepErrorHandlerData errorData);
}