using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
#endif

namespace Damdor.Sequentio
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class SequencePlayer : MonoBehaviour, ISequenceDefinition, ISequencePlayer
    {
        public SequenceSettings Settings => settings;
        public IReadOnlyList<StepDefinition> Steps => definition.Steps;
        public VarioStorage Storage => definition.Storage;

        public SequencePlayerState State => InnerSequence?.State ?? SequencePlayerState.Idle;
        public SequenceExecution Execution => InnerSequence?.Execution;
        public float Duration => InnerSequence?.Duration ?? 0f;
        public float CurrentTime => InnerSequence?.CurrentTime ?? 0f;

        private RuntimeSequence innerSequence;
        private bool destroyed;

        [SerializeField] private SequenceSettings settings;
        [SerializeField] private SequenceDefinition definition;

        private RuntimeSequence InnerSequence
        {
            get
            {
                if (destroyed) return innerSequence;
                if (innerSequence == null) innerSequence = RuntimeSequence.Create(this, settings);
                else innerSequence.Settings.CopyFrom(settings);
                return innerSequence;
            }
        }

        public void Init() => InnerSequence?.Init();
        public void Play() => InnerSequence?.Play();
        public void Pause() => InnerSequence?.Pause();
        public void Stop() => InnerSequence?.Stop();
        public void Stop(StopBehaviour stopBehaviour) => InnerSequence?.Stop(stopBehaviour);
        public void MoveToTime(float time) => InnerSequence?.MoveToTime(time);
        public float GetProgress(StepDefinition step) => innerSequence?.GetProgress(step) ?? 0f;
#if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => InnerSequence?.PlayAndWait() ?? UniTask.CompletedTask;
#endif

        protected virtual void Awake()
        {
            if (Settings.AutoPlay == AutoPlayMode.Awake) InnerSequence?.Play();
        }

        protected virtual void Start()
        {
            if (Settings.AutoPlay == AutoPlayMode.Start) InnerSequence?.Play();
        }

        protected virtual void OnEnable()
        {
            if (Settings.AutoPlay == AutoPlayMode.OnEnable) InnerSequence?.Play();
        }

        protected virtual void OnDisable()
        {
            ReleaseRuntimeSequence();
        }
        
        protected virtual void OnDestroy()
        {
            destroyed = true;
            ReleaseRuntimeSequence();
        }

        private void ReleaseRuntimeSequence()
        {
            InnerSequence?.Release();
            innerSequence = null;
        }
    }
}