using System.Collections.Generic;

namespace Damdor.Sequentio
{
    internal static class SequentioPooling
    {
        private static readonly Stack<SequenceRunner> runnerPool = new();
        private static readonly Stack<RawSequence> independentSequencePool = new();
        private static readonly Stack<SequenceSettings> settingsPool = new();
        
        public static SequenceRunner PopRunner()
        {
            return runnerPool.Count > 0 
                ? runnerPool.Pop() 
                : new SequenceRunner();
        }

        public static void ReleaseRunner(SequenceRunner runner)
        {
            if (runner == null) return;
            runner.Reset();
            runnerPool.Push(runner);
        }
        
        public static RawSequence PopRawSequence()
        {
            return independentSequencePool.Count > 0 
                ? independentSequencePool.Pop() 
                : new RawSequence();
        }

        public static void ReleaseRawSequence(RawSequence sequence)
        {
            if (sequence == null) return;
            sequence.Reset();
            independentSequencePool.Push(sequence);
        }

        public static SequenceSettings PopSettings()
        {
            return settingsPool.Count > 0 
                ? settingsPool.Pop() 
                : new SequenceSettings();
        }

        public static void ReleaseSettings(SequenceSettings settings)
        {
            if (settings == null) return;
            settings.Reset();
            settingsPool.Push(settings);
        }
    }
}