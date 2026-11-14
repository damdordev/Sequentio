using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class LogEventStepPlayer : EventStepPlayer<LogEventStepDefinition>
    {
        private LogType logType;
        private string message;

        protected override void Init(LogEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            logType = definition.LogType;
            message = definition.Message.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Exception:
                case LogType.Assert:
                    Debug.LogException(new System.Exception(message));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}