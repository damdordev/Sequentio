using Damdor.Vario;
using UnityEngine;
#if DAMDOR_SEQUENTIO_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a reusable sequence asset with component settings and variable overrides.
    /// </summary>
    [ExecuteAlways]
    public sealed class SequenceAssetPlayer : MonoBehaviour, ISequencePlayer, IVarioStorageSource
    {
        /// <summary>
        /// The sequence asset to play. 
        /// </summary>
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

        /// <summary>
        /// The playback settings for this sequence.
        /// </summary>
        public SequenceSettings Settings => settings;
        VarioStorage IVarioStorageSource.Storage => storageOverride;
        
        /// <summary>
        /// The variables copied over the nested or asset sequence storage when compiling its playback plan.
        /// </summary>
        public VarioStorage StorageOverride => storageOverride;
        
        /// <summary>
        /// The current playback state.
        /// </summary>
        public SequencePlayerState State => InnerSequence?.State ?? SequencePlayerState.Idle;
        
        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        public float Duration => InnerSequence?.Duration ?? 0f;
        
        /// <summary>
        /// The current position on the sequence timeline, in seconds.
        /// </summary>
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

        /// Starts playback to create the execution, then pauses it. It should be used to
        /// initialize the scene according to the sequence's initial state. 
        public void Init() => InnerSequence?.Init();
        
        /// <summary>
        /// Starts playback, resumes a paused sequence, or restarts a finished sequence.
        /// </summary>
        public void Play() => InnerSequence?.Play();
        
        /// <summary>
        /// Pauses playback if the sequence is currently playing.
        /// </summary>
        public void Pause() => InnerSequence?.Pause();
        
        /// <summary>
        /// Stops playback using the configured default stop behavior.
        /// </summary>
        public void Stop() => InnerSequence?.Stop();
        
        /// <summary>
        /// Stops playback using the specified target restoration behavior.
        /// </summary>
        /// <param name="stopBehaviour">Whether to restore captured initial target values or retain current values.</param>
        public void Stop(StopBehaviour stopBehaviour) => InnerSequence?.Stop(stopBehaviour);
        
        /// <summary>
        /// Applies the requested sequence time. Has no effect while the player is idle.
        /// </summary>
        /// <param name="time">The requested sequence time in seconds, clamped to the sequence duration.</param>
        public void MoveToTime(float time) => InnerSequence?.MoveToTime(time);
        
        /// <summary>
        /// Gets the last assigned normalized progress of the specified step.
        /// </summary>
        /// <param name="step">The step definition whose progress or metadata is requested.</param>
        /// <returns>The step progress, or zero if there is no active execution or the step is not present.</returns>
        public float GetProgress(StepDefinition step) => InnerSequence?.GetProgress(step) ?? 0f;

#if DAMDOR_SEQUENTIO_UNITASK
        /// <summary>
        /// Starts playback and waits until the sequence finishes or returns to the idle state.
        /// </summary>
        /// <returns>A task that completes when playback finishes or is stopped.</returns>
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
