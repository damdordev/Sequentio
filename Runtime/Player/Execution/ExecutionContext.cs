namespace Damdor.Sequentio
{
    internal class ExecutionContext : IExecutionContext
    {
        public PlayDirection Direction { get; set; }
        public int LoopIndex { get; set; }

        internal void Reset()
        {
            Direction = PlayDirection.Forward;
            LoopIndex = 0;
        }
    }
}