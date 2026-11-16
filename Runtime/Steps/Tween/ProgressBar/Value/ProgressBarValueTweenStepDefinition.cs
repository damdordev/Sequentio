#if DAMDOR_SEQUENTIO_PROGRESSIO

using System;
using Damdor.Progressio;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ProgressBarValueTweenStepPlayer), "ProgressBar/Value", "Value", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ProgressBarValueTweenStepDefinition : FloatTweenStepDefinition<ProgressBar>
    {
        public VarioValue<bool> Immediate
        {
            get => immediate;
            set => immediate = value;
        }

        [SerializeField] private VarioValue<bool> immediate = true;

        public override void Reset()
        {
            base.Reset();
            immediate = true;
        }
    }
}
#endif