using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequentioStepVisibility
    {
        public VarioStorage Storage { get; set; }
        
        public bool IsEnabled(bool runtime)
        {
            if (!enabled.Evaluate(Storage)) return false;
            if (runtime && !enabledInRuntime.Evaluate(Storage)) return false;
            if (!runtime && !enabledInPreview.Evaluate(Storage)) return false;
            return true;
        }

        internal void Reset()
        {
            Storage = null;
            enabled = VarioValue<bool>.Raw(true);
            enabledInRuntime = VarioValue<bool>.Raw(true);
            enabledInPreview = VarioValue<bool>.Raw(true);
        }

        internal void CopyFrom(SequentioStepVisibility other)
        {
            enabled = other.enabled;
            enabledInRuntime = other.enabledInRuntime;
            enabledInPreview = other.enabledInPreview;
        }

        [SerializeField] private VarioValue<bool> enabled = true;
        [SerializeField] private VarioValue<bool> enabledInRuntime = true;
        [SerializeField] private VarioValue<bool> enabledInPreview = true;
        
    }
}