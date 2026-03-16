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
        
        [SerializeField] private AutoPlayMode autoPlay;
        [SerializeField] private LoopMode loop;
        [SerializeField] private TimescaleMode timescale;
        [SerializeField] private PlayDirection direction;

    }
}