using System;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequenceSettings
    {
        public AutoPlayMode AutoPlay
        {
            get => autoPlay;
            set => autoPlay = value;
        }

        public LoopMode Loop
        {
            get => loop;
            set => loop = value;
        }

        public TimescaleMode Timescale
        {
            get => timescale;
            set => timescale = value;
        }
        
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public PlayDirection Direction
        {
            get => direction;
            set => direction = value;
        }
        
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