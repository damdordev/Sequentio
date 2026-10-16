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
    
    public interface ISequencePlayer
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
        float GetProgress(SequenceStep step);

#if DAMDOR_SEQUENTIO_UNITASK
        UniTask PlayAndWait();
#endif
    }
}