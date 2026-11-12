// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class StepExecution
    {
        public StepDefinition Definition { get; set; }
        public StepPlayer Player { get; set; }
        public bool ValuesApplied { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float Duration { get; set; }
        public bool InitOnStart { get; set; }
        public bool RunWithPreviousStep { get; set; }
        public float Delay { get; set; }
        public float Progress { get; private set; }

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
        
        public void SetProgress(float progress)
        {
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

        public void ReturnToInitialState()
        {
            try
            {
                Player.ReturnToInitialState();
            }
            catch (Exception e)
            {
                NotifyException(SequentioStepErrorSource.ReturnToInitialState, e);
            }
        }

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
            Progress = 0f;
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