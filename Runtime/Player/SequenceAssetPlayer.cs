using Damdor.Vario;
using UnityEngine;
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

        public SequenceSettings Settings => settings;
        public VarioStorage Storage => storageOverride;
        public SequencePlayerState State => InnerSequence?.State ?? SequencePlayerState.Idle;
        public float Duration => InnerSequence?.Duration ?? 0f;
        public float CurrentTime => InnerSequence?.CurrentTime ?? 0f;

        [SerializeField] private SequenceSettings settings;
        [SerializeField] private SequenceAsset asset;
        [SerializeField] private VarioStorage storageOverride = new();

        private SequenceAsset currentAsset;
        private RuntimeSequence innerSequence;
        private bool destroyed;

        private RuntimeSequence InnerSequence
        {
            get
            {
                if (destroyed) return null;
                if (innerSequence != null && currentAsset == asset)
                {
                    innerSequence.Settings.CopyFrom(settings);
                    return innerSequence;
                }

                ReleaseRuntimeSequence();
                currentAsset = asset;
                innerSequence = asset != null
                    ? RuntimeSequence.Create(asset, settings, ApplyStorageOverride)
                    : RuntimeSequence.Create();
                return innerSequence;
            }
        }

        public SequencePlan CreateDefinitionPlan(SequenceSettings settings) => InnerSequence?.CreateDefinitionPlan();
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
            if (Application.isPlaying && asset != null && Settings.AutoPlay == AutoPlayMode.Awake) Play();
        }

        private void Start()
        {
            if (Application.isPlaying && asset != null && Settings.AutoPlay == AutoPlayMode.Start) Play();
        }

        private void OnEnable()
        {
            if (Application.isPlaying && asset != null && Settings.AutoPlay == AutoPlayMode.OnEnable) Play();
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
            if (!Application.isPlaying) innerSequence?.Stop(StopBehaviour.ReturnToInitialState);
            innerSequence?.Release();
            innerSequence = null;
            currentAsset = null;
        }
    }
}
