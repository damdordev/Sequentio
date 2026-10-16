using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    [Serializable]
    [SequentioStep("Sequentio/Run sequence", "Run Sequence", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceStep : SequenceStep
    {
        public override string TargetNameInEditor => GetTargetName(Sequence);

        public VarioValue<SequencePlayer> Sequence
        {
            get => sequence;
            set => sequence = value;
        }
        
        [SerializeField] private VarioValue<SequencePlayer> sequence;

        private SequencePlayer vSequencePlayer;
        private float v_duration;
        private float v_speed;
        private bool duringAnimation;
        
        protected override float CalculateDuration()
        {
            InitSequenceIfNeeded();

            return v_duration;
        }

        public override void Init()
        {
            base.Init();
            duringAnimation = true;
            vSequencePlayer = null;
            InitSequenceIfNeeded();
        }

        public override void Release()
        {
            base.Release();
            vSequencePlayer = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            duringAnimation = false;
            if(vSequencePlayer != null) vSequencePlayer.Stop(StopBehaviour.ReturnToInitialState);
            vSequencePlayer = null;
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            vSequencePlayer?.MoveToTime(t*v_duration*v_speed);
        }

        public override void InvalidateDuration()
        {
            base.InvalidateDuration();
            vSequencePlayer = null;
        }

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not RunSequenceStep other) return;

            sequence = other.sequence;
        }

        internal override void Reset()
        {
            base.Reset();

            sequence = null;
            vSequencePlayer = null;
            duringAnimation = false;
        }

        private void InitSequenceIfNeeded()
        {
            var newSequence = sequence.Evaluate(Storage);
            if (newSequence == vSequencePlayer) return;
            vSequencePlayer?.Stop(StopBehaviour.ReturnToInitialState);
            vSequencePlayer = newSequence;

            if (vSequencePlayer == null)
            {
                v_duration = 0f;
                return; 
            }

            var isIdle = vSequencePlayer.State == SequenceState.Idle;
            if(isIdle) vSequencePlayer.Init();
            v_speed = vSequencePlayer.Settings.Speed;
            v_duration = v_speed > Mathf.Epsilon ? vSequencePlayer.Duration / v_speed : 0f;
            if(isIdle && !duringAnimation) vSequencePlayer.Stop(StopBehaviour.ReturnToInitialState);
        }
        
    }
}