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

        public VarioValue<Sequence> Sequence
        {
            get => sequence;
            set => sequence = value;
        }
        
        [SerializeField] private VarioValue<Sequence> sequence;

        private Sequence v_sequence;
        private float v_duration;
        private bool duringAnimation;
        
        public override float CalculateDuration()
        {
            InitSequenceIfNeeded();

            return v_duration;
        }

        public override void Init()
        {
            base.Init();
            duringAnimation = true;
            v_sequence = null;
            InitSequenceIfNeeded();
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            duringAnimation = false;
            if(v_sequence != null) v_sequence.Stop(StopBehaviour.ReturnToInitialState);
            v_sequence = null;
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            v_sequence?.MoveToTime(t*v_duration);
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
            v_sequence = null;
            duringAnimation = false;
        }

        private void InitSequenceIfNeeded()
        {
            var newSequence = sequence.Evaluate(Storage);
            if (newSequence == v_sequence) return;
             
            v_sequence?.Stop(StopBehaviour.ReturnToInitialState);
            v_sequence = newSequence;

            if (v_sequence == null)
            {
                v_duration = 0f;
                return; 
            }

            var isIdle = v_sequence.State == SequenceState.Idle;
            if(isIdle) v_sequence.Init();
            v_duration = v_sequence.Duration;
            if(isIdle && !duringAnimation) v_sequence.Stop(StopBehaviour.ReturnToInitialState);
        }
        
    }
}