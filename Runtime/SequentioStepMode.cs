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
            if (!Storage.Evaluate(enabled)) return false;
            if (runtime && !Storage.Evaluate(enabledInRuntime)) return false;
            if (!runtime && !Storage.Evaluate(enabledInPreview)) return false;
            return true;
        }

        [SerializeField] private VarioValue<bool> enabled = true;
        [SerializeField] private VarioValue<bool> enabledInRuntime = true;
        [SerializeField] private VarioValue<bool> enabledInPreview = true;
    }
}