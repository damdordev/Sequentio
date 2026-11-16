using System;
using UnityEngine;
using UnityEngine.Events;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RunEventStepPlayer), "RunEvent", "Run Event", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RunEventStepDefinition : EventStepDefinition
    {
        public UnityEvent Event
        {
            get => unityEvent;
            set => unityEvent = value;
        }

        [SerializeField] private UnityEvent unityEvent = new();
    }
}
