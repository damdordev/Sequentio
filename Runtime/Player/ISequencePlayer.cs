#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global
#endif

namespace Damdor.Sequentio
{
    public interface ISequencePlayer
    {
        SequencePlayerState State { get; }
        float Duration { get; }
        float CurrentTime { get; }
        void Init();
        void Play();
        void Pause();
        void Stop();
        void Stop(StopBehaviour stopBehaviour);
        void MoveToTime(float time);
        float GetProgress(StepDefinition step);

#if DAMDOR_SEQUENTIO_UNITASK
        UniTask PlayAndWait();
#endif
    }
}