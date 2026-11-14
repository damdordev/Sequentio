namespace Damdor.Sequentio
{
    public interface IExecutionContext
    {
        public PlayDirection Direction { get; }
        public int LoopIndex { get; }
    }
}