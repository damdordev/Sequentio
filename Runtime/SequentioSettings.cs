using System;
using System.Collections.Generic;

namespace Damdor.Sequentio
{
    public static class SequentioSettings
    {
        public static IReadOnlyList<Type> SupportedStepTypes
        {
            get
            {
                EnsureInit();
                return supportedStepTypes;
            }
        }
        
        private static readonly List<Type> supportedStepTypes = new();
        private static bool init;
        
        public static void ResetToInitialSettings()
        {
            ResetSupportedStateTypes();
            init = true;
        }
        
        public static void RegisterStateType(Type type)
        {
            EnsureInit();
            supportedStepTypes.Add(type);
        }
        
        private static void ClearSupportedStateTypes()
        {
            supportedStepTypes.Clear();
        }
        
        private static void ResetSupportedStateTypes()
        {
            supportedStepTypes.Clear();
            
            supportedStepTypes.Add(typeof(PositionTweenStep));
        }
        
        private static void EnsureInit()
        {
            if (init) return;
            ResetToInitialSettings();
        }
        
    }
}