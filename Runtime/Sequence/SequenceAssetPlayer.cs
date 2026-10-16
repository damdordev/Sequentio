#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceAssetPlayer : MonoBehaviour, ISequencePlayer
    {
        public VarioStorage Storage => overrideStorage;

        public SequenceState State
        {
            get
            {
                if(runtimeSequencePlayer == null) PrepareSequence();
                return runtimeSequencePlayer?.State ?? SequenceState.Idle;   
            }
        }
        public SequenceSettings Settings
        {
            get
            {
                if(runtimeSequencePlayer == null) PrepareSequence();
                return runtimeSequencePlayer?.Settings;
            }
        }

        public float Duration
        {
            get
            {
                if(runtimeSequencePlayer == null) PrepareSequence();
                return runtimeSequencePlayer?.Duration ?? 0f;
            }
        }

        public float CurrentTime => runtimeSequencePlayer?.CurrentTime ?? 0f;

        [SerializeField] private SequenceDefinitionAsset asset;
        [SerializeField] private VarioStorage overrideStorage;
        
        private RuntimeSequencePlayer runtimeSequencePlayer;

        private void Awake()
        {
            if(asset != null && asset.Settings.AutoPlay == AutoPlayMode.Awake)
            {
                PrepareSequence();
            }
        }
        
        private void Start()
        {
            if(asset != null && asset.Settings.AutoPlay == AutoPlayMode.Start)
            {
                PrepareSequence();
            }
        }
        
        private void OnEnable()
        {
            if(asset != null && asset.Settings.AutoPlay == AutoPlayMode.OnEnable)
            {
                if(runtimeSequencePlayer == null) PrepareSequence();
                else runtimeSequencePlayer?.Play();
            }
        }
        
        public void Init()
        {
            if(NeedRebuildRawSequenceOnInit()) PrepareSequence();
            runtimeSequencePlayer.Init();
        }

        public void Play()
        {
            if(NeedRebuildRawSequenceOnInit()) PrepareSequence();
            runtimeSequencePlayer?.Play();
        }
        
        public void Pause()
        {
            runtimeSequencePlayer?.Pause();
        }

        public void Stop()
        {
            runtimeSequencePlayer?.Stop();
            ReleaseSequence();
        }

        public void Stop(StopBehaviour stopBehaviour)
        {
            runtimeSequencePlayer?.Stop(stopBehaviour);
            ReleaseSequence();
        }

        public void MoveToTime(float time)
        {
            runtimeSequencePlayer.MoveToTime(time);
        }

        public float GetProgress(SequenceStep step) => runtimeSequencePlayer.GetProgress(step);

#if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => runtimeSequencePlayer.PlayAndWait();
#endif

        private void PrepareSequence()
        {
            ReleaseSequence();
            if (asset == null) return;
            runtimeSequencePlayer = RuntimeSequencePlayer.Create(asset, false);
            runtimeSequencePlayer.Storage.CopyFrom(overrideStorage);
            if(runtimeSequencePlayer.Settings.AutoPlay != AutoPlayMode.None) runtimeSequencePlayer.Play();
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
            runtimeSequencePlayer?.Release();
            runtimeSequencePlayer = null;
        }
        
        private void UpdateTime(float dt)
        {
            runtimeSequencePlayer?.UpdateTime(dt);
        }
        
        private bool NeedRebuildRawSequenceOnInit()
        {
            if (runtimeSequencePlayer == null) return true;
            if (runtimeSequencePlayer.State == SequenceState.Playing || runtimeSequencePlayer.State == SequenceState.Paused) return false;
            return true;
        }
        
    }
}