using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable] 
    [SequentioStep(typeof(LogEventStepPlayer), "Log", "Log", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LogEventStepDefinition : EventStepDefinition
    {
        public LogType LogType
        {
            get => logType; 
            set => logType = value;
        }
        
        public VarioValue<string> Message
        {
            get => message; 
            set => message = value;
        }
        
        [SerializeField] private LogType logType;
        [SerializeField] private VarioValue<string> message;

        public override void Reset()
        {
            base.Reset();
            logType = LogType.Log;
            message = "";
        }
    }
}