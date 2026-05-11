using System.Collections.Generic;
using UnityEngine;

namespace Damdor.Sequentio
{
    internal class SequentioUpdateProvider : MonoBehaviour
    {
        public delegate void UpdateReceiver(float deltaTime, float unscaledDeltaTime);

        private static readonly List<UpdateReceiver> receivers = new();
        private static readonly List<UpdateReceiver> tmpReceivers = new();
        private static SequentioUpdateProvider instance;

        public static void Register(UpdateReceiver receiver)
        {
            if (receiver == null || !Application.isPlaying) return;
            if (instance == null) CreateInstance();
            receivers.Add(receiver);
        }

        public static void Unregister(UpdateReceiver receiver)
        {
            receivers.Remove(receiver);
            if (receivers.Count == 0) DestroyInstance();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetState()
        {
            receivers.Clear();
            instance = null;
        }

        private void Update()
        {
            tmpReceivers.Clear();
            tmpReceivers.AddRange(receivers);
            foreach (var updateReceiver in tmpReceivers)
            {
                updateReceiver(Time.deltaTime, Time.unscaledDeltaTime);
            }
            tmpReceivers.Clear();
        }

        private static void CreateInstance()
        {
            var go = new GameObject("SequentioBarUpdateProvider")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            DontDestroyOnLoad(go);
            go.AddComponent<SequentioUpdateProvider>();
            instance = go.GetComponent<SequentioUpdateProvider>();
        }

        private static void DestroyInstance()
        {
            if (instance == null) return;
            Destroy(instance.gameObject);
            instance = null;
        }
        
    }
}