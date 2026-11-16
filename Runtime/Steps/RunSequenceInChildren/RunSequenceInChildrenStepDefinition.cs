using System;
using System.Linq;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RunSequenceInChildrenStepPlayer), "Sequentio/Run sequence in children", "Run Sequence In Children", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceInChildrenStepDefinition : StepDefinition
    {
        public VarioValue<GameObject> Root
        {
            get => root;
            set => root = value;
        }

        public BaseRunSequenceInChildrenGetter Getter
        {
            get => getter;
            set => getter = value;
        }
        
        public BaseRunSequenceInChildrenFilter Filter
        {
            get => filter;
            set => filter = value;
        }
        
        public BaseRunSequenceInChildrenSorter Sorter
        {
            get => sorter;
            set => sorter = value;
        }
        
        public BaseRunSequenceInChildrenScheduler Scheduler
        {
            get => scheduler;
            set => scheduler = value;
        }

        public override float CalculateDuration(VarioStorage storage)
        {
            var calculatedRoot = root.Evaluate(storage);
            if (calculatedRoot == null) return 0f;
            RunSequenceInChildrenHelper.Schedule(storage, calculatedRoot, getter, filter, sorter, scheduler, schedule);
            
            if (schedule.Sequences.Count == 0) return 0f;
            var duration = schedule.Duration;
            schedule.Reset();
            return duration;
        }

        [SerializeField]
        [SequenceStepParameterGroup(SequenceStepParameterGroups.Default)] 
        private VarioValue<GameObject> root;
        
        [SerializeReference]
        [SequenceStepParameterGroup(SequenceStepParameterGroups.Default)]
        [SerializeRunSequenceInChildrenGetterSequenceParameter]
        private BaseRunSequenceInChildrenGetter getter;
        
        [SerializeReference]
        [SequenceStepParameterGroup(SequenceStepParameterGroups.Default)] 
        [SerializeRunSequenceInChildrenFilterSequenceParameter]
        private BaseRunSequenceInChildrenFilter filter;
        
        [SerializeReference]
        [SequenceStepParameterGroup(SequenceStepParameterGroups.Default)]
        [SerializeRunSequenceInChildrenSorterSequenceParameter]
        private BaseRunSequenceInChildrenSorter sorter;
        
        [SerializeReference]
        [SequenceStepParameterGroup(SequenceStepParameterGroups.Default)] 
        [SerializeRunSequenceInChildrenSchedulerSequenceParameter]
        private BaseRunSequenceInChildrenScheduler scheduler;

        private readonly RunSequenceInChildrenSchedule schedule = new();

        public override void Reset()
        {
            base.Reset();
            schedule.Reset();
            root = null;
            getter = new GetChildrenFromPath();
            filter = null;
            sorter = null;
            scheduler = new BaseRunWithRegularDelayScheduler();
        }

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(root, storage);
    }
}