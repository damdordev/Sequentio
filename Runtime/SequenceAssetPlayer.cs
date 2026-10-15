#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceAssetPlayer : MonoBehaviour, ISequence
    {
        public VarioStorage Storage => overrideStorage;

        public SequenceState State
        {
            get
            {
                if(rawSequence == null) PrepareSequence();
                return rawSequence?.State ?? SequenceState.Idle;   
            }
        }
        public SequenceSettings Settings
        {
            get
            {
                if(rawSequence == null) PrepareSequence();
                return rawSequence?.Settings;
            }
        }

        public float Duration
        {
            get
            {
                if(rawSequence == null) PrepareSequence();
                return rawSequence?.Duration ?? 0f;
            }
        }

        public float CurrentTime => rawSequence?.CurrentTime ?? 0f;

        [SerializeField] private SequenceAsset asset;
        [SerializeField] private VarioStorage overrideStorage;
        
        private RawSequence rawSequence;
        public void Init()
        {
            if(NeedRebuildRawSequenceOnInit()) PrepareSequence();
            rawSequence.Init();
        }

        public void Play()
        {
            if(NeedRebuildRawSequenceOnInit()) PrepareSequence();
            rawSequence?.Play();
        }
        
        public void Pause()
        {
            rawSequence?.Pause();
        }

        public void Stop()
        {
            rawSequence?.Stop();
            ReleaseSequence();
        }

        public void Stop(StopBehaviour stopBehaviour)
        {
            rawSequence?.Stop(stopBehaviour);
            ReleaseSequence();
        }

        public void MoveToTime(float time)
        {
            rawSequence.MoveToTime(time);
        }

        public float GetProgress(SequenceStep step) => rawSequence.GetProgress(step);

#if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => rawSequence.PlayAndWait();
#endif

        private void PrepareSequence()
        {
            ReleaseSequence();
            if (asset == null) return;
            rawSequence = RawSequence.Create(asset);
            rawSequence.Storage.CopyFrom(overrideStorage);
            if(rawSequence.Settings.AutoPlay != AutoPlayMode.None) rawSequence.Play();
        }
        
        private void OnDisable()
        {
            if (!Application.isPlaying) Stop();
        }

        private void OnDestroy()
        {
            ReleaseSequence();
        }
        
        private void ReleaseSequence()
        {
            rawSequence?.Release();
            rawSequence = null;
        }
        
        private void UpdateTime(float dt)
        {
            rawSequence?.UpdateTime(dt);
        }
        
        private bool NeedRebuildRawSequenceOnInit()
        {
            if (rawSequence == null) return true;
            if (rawSequence.State == SequenceState.Playing || rawSequence.State == SequenceState.Paused) return false;
            return true;
        }
        
    }
}