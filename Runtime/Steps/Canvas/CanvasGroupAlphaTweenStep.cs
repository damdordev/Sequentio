using System;
using UnityEngine;

namespace Damdor.Sequentio.Canvas
{
    [Serializable]
    [SequentioStep("CanvasGroup/Alpha", "Change alpha")]
    public class CanvasGroupAlphaTweenStep : FloatTweenStep<CanvasGroup>
    {
        protected override float GetValue(CanvasGroup target) => target.alpha;
        protected override void SetValue(CanvasGroup target, float value) => target.alpha = value;
    }
}