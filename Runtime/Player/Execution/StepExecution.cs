// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable UnusedAutoPropertyAccessor.Global
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

        public void SetProgress(float progress)
        {
            Progress = progress;
            Player.SetProgress(progress);
        }

        public void Release()
        {
            SequentioPooling.ReleaseStepPlayerPlan(this);
        }

        internal void Reset()
        {
            Definition = null;
            Player?.Release();
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
    }
}