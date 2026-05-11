using System;
using UnityEngine;

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

        public PlayDirection Direction
        {
            get => direction;
            set => direction = value;
        }

        internal SequenceSettings Clone() => new()
        {
            autoPlay = autoPlay,
            loop = loop,
            timescale = timescale,
            direction = direction
        };

        internal void Reset()
        {
            autoPlay = AutoPlayMode.None;
            loop = LoopMode.None;
            timescale = TimescaleMode.Normal;
            direction = PlayDirection.Normal;
        }

        internal void Release()
        {
            SequentioPooling.ReleaseSettings(this);
        }

        [SerializeField] private AutoPlayMode autoPlay;
        [SerializeField] private LoopMode loop;
        [SerializeField] private TimescaleMode timescale;
        [SerializeField] private PlayDirection direction;

    }
}