// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Tracks one step during playback and reports errors raised by its player.
    /// </summary>
    public class StepExecution
    {
        /// <summary>
        /// The step definition associated with this plan or execution.
        /// </summary>
        public StepDefinition Definition { get; set; }
       
        /// <summary>
        /// The player that applies this step, or null when no player is available.
        /// </summary>
        public StepPlayer Player { get; set; }
        
        /// <summary>
        /// Whether the step has been initialized and marked for value application.
        /// </summary>
        public bool ValuesApplied { get; set; }
       
        /// <summary>
        /// The time in seconds at which the step starts on the sequence timeline.
        /// </summary>
        public float StartTime { get; set; }
       
        /// <summary>
        /// The time in seconds at which the step ends on the sequence timeline.
        /// </summary>
        public float EndTime { get; set; }
        
        /// <summary>
        /// The evaluated step duration in seconds, excluding its delay.
        /// </summary>
        public float Duration { get; set; }
       
        /// <summary>
        /// Whether to initialize the step when the sequence executor is created, before its scheduled start.
        /// </summary>
        public bool InitOnStart { get; set; }
      
        /// <summary>
        /// Whether this step shares the preceding step group instead of starting a new group.
        /// </summary>
        public bool RunWithPreviousStep { get; set; }
       
        /// <summary>
        /// The delay in seconds from the start of the step group to the start of this step.
        /// </summary>
        public float Delay { get; set; }
        
        /// <summary>
        /// The most recently assigned normalized step progress.
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// Initializes the step player and reports any exception through Sequentio error handling.
        /// </summary>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        public void Init(VarioStorage storage)
        {
            try
            {
                Player.Init(storage);
            }
            catch (Exception e)
            {
                NotifyException(SequentioStepErrorSource.InitPlayer, e);
            }
        }
        
        /// <summary>
        /// Applies normalized progress to the step.
        /// </summary>
        /// <param name="progress">The normalized step progress, where zero is the start and one is the end.</param>
        public void SetProgress(float progress)
        {
            if (Mathf.Approximately(Progress, progress)) return;
            Progress = progress;
            try
            {
                Player.SetProgress(progress);
            }
            catch (Exception e)
            {
                NotifyException(SequentioStepErrorSource.SetProgress, e);
            }
        }

        /// <summary>
        /// Restores target values captured during step initialization.
        /// </summary>
        public void ReturnToInitialState()
        {
            try
            {
                Progress = 0f;
                Player.ReturnToInitialState();
            }
            catch (Exception e)
            {
                NotifyException(SequentioStepErrorSource.ReturnToInitialState, e);
            }
        }

        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
        public void Release()
        {
            SequentioPooling.ReleaseStepExecution(this);
        }

        internal void Reset()
        {
            Definition = null;
            if (Player != null) SequentioPooling.ReleaseStepPlayer(Player);
            Player = null;
            ValuesApplied = false;
            StartTime = 0f;
            EndTime = 0f;
            Duration = Delay = 0f;
            InitOnStart = false;
            RunWithPreviousStep = false;
            Delay = 0f;
            Progress = -1f;
        }
        
        private void NotifyException(SequentioStepErrorSource source, Exception exception)
        {
            SequentioSettings.NotifyOnStepError(new SequentioStepErrorHandlerData(
                    source,
                    Definition,
                    Player,
                    exception
                )
            );
        }
    }
}