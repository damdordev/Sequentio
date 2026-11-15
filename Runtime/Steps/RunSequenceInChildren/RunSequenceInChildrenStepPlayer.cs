using System.Linq;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class RunSequenceInChildrenStepPlayer : StepPlayer<RunSequenceInChildrenStepDefinition>
    {
        private RunSequenceInChildrenSchedule schedule = new();
        
        private GameObject root;
        private BaseRunSequenceInChildrenGetter getter;
        private BaseRunSequenceInChildrenFilter filter;
        private BaseRunSequenceInChildrenSorter sorter;
        private BaseRunSequenceInChildrenScheduler scheduler;
        
        protected override void Init(RunSequenceInChildrenStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            root = definition.Root.Evaluate(storage);
            getter = definition.Getter;
            filter = definition.Filter;
            sorter = definition.Sorter;
            scheduler = definition.Scheduler;
            if (root == null) return;
            RunSequenceInChildrenHelper.Schedule(storage, root, getter, filter, sorter, scheduler, schedule);
            
            foreach (var sequence in schedule.Sequences)
            {
                sequence.Sequence.Init();
            }
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);

            var time = t * schedule.Duration;
            foreach (var sequence in schedule.Sequences)
            {
                var sequenceTime = time - sequence.StartTime;
                sequence.Sequence.MoveToTime(sequenceTime);
            }
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            
            foreach (var sequence in schedule.Sequences)
            {
                sequence.Sequence.Stop(StopBehaviour.ReturnToInitialState);
            }
        }

        public override void Reset()
        {
            base.Reset();
            schedule.Reset();
        }
    }
}