using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public struct SequentioStepVisibility
    {
        public VarioStorage Storage { get; set; }

        public bool IsEnabled(bool runtime)
        {
            if (!Storage.Evaluate(enabled)) return false;
            if (runtime && !Storage.Evaluate(enabledInRuntime)) return false;
            if (!runtime && !Storage.Evaluate(enabledInPreview)) return false;
            return true;
        }
        
        [SerializeField] private VarioValue<bool> enabled;
        [SerializeField] private VarioValue<bool> enabledInRuntime;
        [SerializeField] private VarioValue<bool> enabledInPreview;
    }
}