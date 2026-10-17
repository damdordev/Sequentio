using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class StepVisibility
    {
        public bool IsEnabled(VarioStorage storage, bool runtime)
        {
            if (!enabled.Evaluate(storage)) return false;
            switch (runtime)
            {
                case true when !enabledInRuntime.Evaluate(storage):
                case false when !enabledInPreview.Evaluate(storage):
                    return false;
                default:
                    return true;
            }
        }

        internal void Reset()
        {
            enabled = true;
            enabledInRuntime = true;
            enabledInPreview = true;
        }

        internal void CopyFrom(StepVisibility other)
        {
            enabled = other.enabled;
            enabledInRuntime = other.enabledInRuntime;
            enabledInPreview = other.enabledInPreview;
        }

        [SerializeField] private VarioValue<bool> enabled;
        [SerializeField] private VarioValue<bool> enabledInRuntime;
        [SerializeField] private VarioValue<bool> enabledInPreview;
        
    }
}