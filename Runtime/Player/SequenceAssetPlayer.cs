using Damdor.Vario;
using UnityEngine;
using UnityEngine.Serialization;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Damdor.Sequentio
{
    [ExecuteAlways]
    public sealed class SequenceAssetPlayer : MonoBehaviour, ISequencePlayer, IVarioStorageSource
    {
        public SequenceAsset Asset
        {
            get => asset;
            set
            {
                if (asset == value) return;
                ReleaseRuntimeSequence();
                asset = value;
            }
        }

        public VarioStorage StorageOverride => storageOverride;
        public VarioStorage Storage => storageOverride;
        public SequencePlayerState State => InnerSequence?.State ?? SequencePlayerState.Idle;
        public SequenceExecution Execution => InnerSequence?.Execution;
        public float Duration => InnerSequence?.Duration ?? 0f;
        public float CurrentTime => InnerSequence?.CurrentTime ?? 0f;

        [SerializeField] private SequenceAsset asset;
        [SerializeField] private VarioStorage storageOverride = new();

        private SequenceAsset currentAsset;
        private RuntimeSequence runtimeSequence;
        private bool destroyed;

        private RuntimeSequence InnerSequence
        {
            get
            {
                if (destroyed) return null;
                if (runtimeSequence != null && currentAsset == asset) return runtimeSequence;

                ReleaseRuntimeSequence();
                currentAsset = asset;
                runtimeSequence = asset != null
                    ? RuntimeSequence.Create(asset, ApplyStorageOverride)
                    : RuntimeSequence.Create();
                return runtimeSequence;
            }
        }

        public SequencePlan CreateDefinitionPlan() => InnerSequence?.CreateDefinitionPlan();
        public void Init() => InnerSequence?.Init();
        public void Play() => InnerSequence?.Play();
        public void Pause() => InnerSequence?.Pause();
        public void Stop() => InnerSequence?.Stop();
        public void Stop(StopBehaviour stopBehaviour) => InnerSequence?.Stop(stopBehaviour);
        public void MoveToTime(float time) => InnerSequence?.MoveToTime(time);
        public float GetProgress(StepDefinition step) => InnerSequence?.GetProgress(step) ?? 0f;
#if DAMDOR_SEQUENTIO_UNITASK
        public UniTask PlayAndWait() => InnerSequence?.PlayAndWait() ?? UniTask.CompletedTask;
#endif

        private void ApplyStorageOverride(VarioStorage storage, SequenceSettings settings)
        {
            storage.CopyFrom(storageOverride);
        }

        private void Awake()
        {
            if (Application.isPlaying && asset != null && asset.Settings.AutoPlay == AutoPlayMode.Awake) Play();
        }

        private void Start()
        {
            if (Application.isPlaying && asset != null && asset.Settings.AutoPlay == AutoPlayMode.Start) Play();
        }

        private void OnEnable()
        {
            if (Application.isPlaying && asset != null && asset.Settings.AutoPlay == AutoPlayMode.OnEnable) Play();
        }

        private void OnDisable()
        {
            ReleaseRuntimeSequence();
        }

        private void OnDestroy()
        {
            destroyed = true;
            ReleaseRuntimeSequence();
        }

        private void ReleaseRuntimeSequence()
        {
            if (!Application.isPlaying) runtimeSequence?.Stop(StopBehaviour.ReturnToInitialState);
            runtimeSequence?.Release();
            runtimeSequence = null;
            currentAsset = null;
        }
    }
}
