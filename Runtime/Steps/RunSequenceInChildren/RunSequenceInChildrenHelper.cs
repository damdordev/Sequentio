using Damdor.Vario;
using UnityEngine;
using UnityEngine.Pool;

namespace Damdor.Sequentio
{
    public static class RunSequenceInChildrenHelper
    {
        public static void Schedule(
            VarioStorage storage,
            GameObject root,
            BaseRunSequenceInChildrenGetter getter,
            BaseRunSequenceInChildrenFilter filter,
            BaseRunSequenceInChildrenSorter sorter,
            BaseRunSequenceInChildrenScheduler scheduler,
            RunSequenceInChildrenSchedule result)
        {
            var players = ListPool<ISequencePlayer>.Get();

            try
            {
                getter?.GetSequences(storage, root, players);
                if (filter != null)
                {
                    players.RemoveAll(p => !filter.Check(storage, p));
                }
                sorter?.Sort(storage, players);
                scheduler?.Schedule(storage, players, result);
            }
            finally
            {
                ListPool<ISequencePlayer>.Release(players);
            }
        }
    }
}