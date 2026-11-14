using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    /// <summary>
    /// Provides lifecycle hooks for initializing, updating, restoring, and recycling a sequence step.
    /// </summary>
    public class StepPlayer
    {
        /// <summary>
        /// Resets the player state and references for reuse.
        /// </summary>
        public virtual void Reset()
        {
            ExecutionContext = null;
        }

        /// <summary>
        /// Initializes playback or captures the step values needed for execution.
        /// </summary>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        public virtual void Init(VarioStorage storage, IExecutionContext executionContext)
        {
            ExecutionContext = executionContext;
        }
       
        /// <summary>
        /// Applies normalized progress to the step.
        /// </summary>
        /// <param name="t">The normalized progress or interpolation factor.</param>
        public virtual void SetProgress(float t){}
        
        /// <summary>
        /// Restores target values captured during step initialization.
        /// </summary>
        public virtual void ReturnToInitialState(){}
        
        protected virtual IExecutionContext ExecutionContext { get; private set; }
    }
    
    /// <summary>
    /// Provides a step player associated with a strongly typed step definition.
    /// </summary>
    /// <typeparam name="TStepDefinition">The step definition type handled by the player or compiler.</typeparam>
    public class StepPlayer<TStepDefinition> : StepPlayer where TStepDefinition : StepDefinition
    {
        private TStepDefinition definition;

        /// <summary>
        /// Associates this instance with the supplied definition or serialized object.
        /// </summary>
        /// <param name="definition">The definition used to configure playback.</param>
        public void Setup(TStepDefinition definition)
        {
            this.definition = definition;
        }
        
        /// <summary>
        /// Initializes playback or captures the step values needed for execution.
        /// </summary>
        /// <param name="definition">The definition used to configure playback.</param>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        protected virtual void Init(TStepDefinition definition, VarioStorage storage){}

        /// <inheritdoc />
        public sealed override void Init(VarioStorage storage, IExecutionContext executionContext)
        {
            base.Init(storage, executionContext);
            Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            definition = null;
        }
    }
}