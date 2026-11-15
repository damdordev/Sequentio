using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenGetter("Pick directly")]
    public class PickChildrenDirectly : BaseRunSequenceInChildrenGetter
    {
        public List<SequencePlayerData> Sequences => sequences;
        
        [Serializable]
        public struct SequencePlayerData
        {
            public SequencePlayer Player
            {
                get => player;
                set => player = value;
            }
            
            public SequenceAssetPlayer AssetPlayer
            {
                get => assetPlayer;
                set => assetPlayer = value;
            }
            
            [SerializeField] private SequencePlayer player;
            [SerializeField] private SequenceAssetPlayer assetPlayer;
        }
        
        public override void GetSequences(VarioStorage storage, GameObject root, List<ISequencePlayer> sequences)
        {
            foreach (var sequenceData in this.sequences)
            {
                if(sequenceData.Player != null) sequences.Add(sequenceData.Player);
                if(sequenceData.AssetPlayer != null) sequences.Add(sequenceData.AssetPlayer);
            }
        }

        [SerializeField] private List<SequencePlayerData> sequences;

    }
}