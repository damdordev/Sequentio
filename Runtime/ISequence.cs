#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public enum SequenceState
    {
        Idle,
        Playing,
        Paused,
        Finished
    }
    
    public interface ISequence
    {
        VarioStorage Storage { get; }
        SequenceState State { get; }
        SequenceSettings Settings { get; }
        float Duration { get; }
        float CurrentTime { get; }
        void Init();
        void Play();
        void Pause();
        void Stop();
        void Stop(StopBehaviour stopBehaviour);
        void MoveToTime(float time);
        void Release();
        void AddStep(SequenceStep step);
        float GetProgress(int stepNumber);

#if DAMDOR_SEQUENTIO_UNITASK
        UniTask PlayAndWait();
#endif
    }
}