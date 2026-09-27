using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class SequenceAssetPreview : MonoBehaviour, IVarioStorageSource
    {
        public SequenceAsset Asset => asset;
        public VarioStorage Storage => storage;
        
        [SerializeField] private SequenceAsset asset;
        [SerializeField] private VarioStorage storage;
    }
}