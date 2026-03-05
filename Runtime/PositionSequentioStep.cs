using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class PositionSequentioStep : SequentioStep
    {
        [SerializeField] private VarioValue<TimeSpan, SerializableTimeSpan> duration;
        [SerializeField] private VarioValue<Vector3> initPosition;
        [SerializeField] private VarioValue<Vector3> targetPosition;

        private float v_duration;
        private Vector3 v_initPosition;
        private Vector3 v_targetPosition;
        
        public override void CalculateDuration()
        {
            base.CalculateDuration();

            v_duration = (float) Storage.Evaluate(duration).TotalSeconds;
        }

        public override void ApplyValues()
        {
            base.ApplyValues();
            
            v_initPosition = Storage.Evaluate(initPosition);
            v_targetPosition = Storage.Evaluate(targetPosition);
        }
    }
}