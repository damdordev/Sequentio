using System;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Stores a shake randomness mode in Vario variable storage.
    /// </summary>
    [Serializable]
    [VarioVariable("shakeRandomnessMode")]
    public class ShakeRandomnessModeVariable : VarioVariable<ShakeRandomnessMode>
    {
    }
}