namespace Damdor.Sequentio
{
    public static class RunSequenceInChildrenSettings
    {
        public static SerializedStepComponent<BaseRunSequenceInChildrenGetter> Getters { get; } = new();
        public static SerializedStepComponent<BaseRunSequenceInChildrenFilter> Filters { get; } = new();
        public static SerializedStepComponent<BaseRunSequenceInChildrenSorter> Sorters { get; } = new();
        public static SerializedStepComponent<BaseRunSequenceInChildrenScheduler> Schedulers { get; } = new();
    }
}