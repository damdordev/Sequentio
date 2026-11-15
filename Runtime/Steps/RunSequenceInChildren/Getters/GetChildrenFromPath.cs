using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenGetter("Get from path")]
    public class GetChildrenFromPath : BaseRunSequenceInChildrenGetter
    {
        public VarioValue<string> Path
        {
            get => path;
            set => path = value;
        }
        
        [SerializeField] private VarioValue<string> path;
        
        public override void GetSequences(VarioStorage storage, GameObject root, List<ISequencePlayer> sequences)
        {
            var calculatedPath = path.Evaluate(storage);
            var transform = root.transform;
            for (var i = 0; i < transform.childCount; ++i)
            {
                var player = Get(transform.GetChild(i).gameObject, calculatedPath);
                if(player != null) sequences.Add(player);
            }
        }
        
        private static ISequencePlayer Get(GameObject go, string path)
        {
            if(string.IsNullOrEmpty(path)) return go.GetComponent<ISequencePlayer>();
            var parts = path.Split("/");

            var transform = go.transform;
            foreach(var part in parts)
            {
                var partName = part;
                var useChild = false;
                var childIndex = -1;
                var beginIndex = part.IndexOf('[');
                var endIndex = part.IndexOf(']');
                
                if(beginIndex >= 0 && endIndex > 0)
                {
                    useChild = true;
                    var number = part.Substring(beginIndex + 1, endIndex - beginIndex - 1);
                    childIndex = number[0] == '^' ? int.Parse(number.Substring(1)) : int.Parse(number);
                    partName = partName.Substring(0, beginIndex);
                }
                
                if(!string.IsNullOrWhiteSpace(partName)) transform = transform.Find(partName);
                if(useChild) transform = transform.GetChild(childIndex);
            }

            return transform != null ? transform.GetComponent<ISequencePlayer>() : null;
        }
    }
}