using System;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    /// <summary>
    /// Configures automatic playback, looping, timing, speed, direction, and stopping.
    /// </summary>
    [Serializable]
    public class SequenceSettings
    {
        /// <summary>
        /// The Unity lifecycle callback that starts playback automatically.
        /// </summary>
        public AutoPlayMode AutoPlay
        {
            get => autoPlay;
            set => autoPlay = value;
        }

        /// <summary>
        /// The behavior used when playback reaches a sequence boundary.
        /// </summary>
        public LoopMode Loop
        {
            get => loop;
            set => loop = value;
        }

        /// <summary>
        /// Whether playback uses scaled or unscaled elapsed time.
        /// </summary>
        public TimescaleMode Timescale
        {
            get => timescale;
            set => timescale = value;
        }
        
        /// <summary>
        /// The multiplier applied to elapsed playback time.
        /// </summary>
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>
        /// The initial playback direction.
        /// </summary>
        public PlayDirection Direction
        {
            get => direction;
            set => direction = value;
        }
        
        /// <summary>
        /// The target restoration behavior used by the parameterless stop operation.
        /// </summary>
        public StopBehaviour DefaultStopBehaviour
        {
            get => defaultStopBehaviour;
            set => defaultStopBehaviour = value;
        }

        internal SequenceSettings Clone()
        {
            var clone = SequentioPooling.PopSequenceSettings();
            clone.AutoPlay = AutoPlay;
            clone.Loop = Loop;
            clone.Timescale = Timescale;
            clone.Direction = Direction;
            clone.DefaultStopBehaviour = DefaultStopBehaviour;
            clone.speed = speed;
            return clone;
        }

        /// <summary>
        /// Copies all playback options from another settings instance.
        /// </summary>
        /// <param name="otherSettings">The settings from which all playback options are copied.</param>
        public void CopyFrom(SequenceSettings otherSettings)
        {
            AutoPlay = otherSettings.AutoPlay;
            Loop = otherSettings.Loop;
            Timescale = otherSettings.Timescale;
            Direction = otherSettings.Direction;
            DefaultStopBehaviour = otherSettings.DefaultStopBehaviour;
            speed = otherSettings.speed;
        }

        internal void Reset()
        {
            autoPlay = AutoPlayMode.None;
            loop = LoopMode.None;
            timescale = TimescaleMode.Normal;
            direction = PlayDirection.Forward;
            defaultStopBehaviour = StopBehaviour.ReturnToInitialState;
            speed = 1f;
        }

        internal void Release()
        {
            SequentioPooling.ReleaseSequenceSettings(this);
        }

        [SerializeField] private AutoPlayMode autoPlay;
        [SerializeField] private LoopMode loop;
        [SerializeField] private TimescaleMode timescale;
        [SerializeField] private float speed = 1f;
        [SerializeField] private PlayDirection direction;
        [SerializeField] private StopBehaviour defaultStopBehaviour;
    }
}