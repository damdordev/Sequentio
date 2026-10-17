using System.Collections.Generic;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    internal class SequentioUpdateProvider : MonoBehaviour
    {
        public delegate void UpdateReceiver(float deltaTime);

        private static readonly List<UpdateReceiver> receivers = new();
        private static readonly List<UpdateReceiver> unscaledReceivers = new();
        private static readonly List<UpdateReceiver> tmpReceivers = new();
        
        private static SequentioUpdateProvider instance;
        private static bool isInstanceCreated;
        private static bool isAttachedToEditorUpdate;
        private static double lastEditorTime;
        
        
#if UNITY_EDITOR
        static SequentioUpdateProvider()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if(state is UnityEditor.PlayModeStateChange.EnteredEditMode or UnityEditor.PlayModeStateChange.EnteredPlayMode) RegisterToUpdate();
        }
#endif

        public static void Register(UpdateReceiver receiver, bool unscaled)
        {
            if (receiver == null) return;
            if(unscaled) unscaledReceivers.Add(receiver);
            else receivers.Add(receiver);
            if(!IsRegisteredToUpdate()) RegisterToUpdate();
        }

        public static void Unregister(UpdateReceiver receiver)
        {
            receivers.Remove(receiver);
            unscaledReceivers.Remove(receiver);
            if (receivers.Count == 0 && unscaledReceivers.Count == 0) UnregisterFromUpdate();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetState()
        {
            receivers.Clear();
            instance = null;
        }

        private void Update()
        {
            Dispatch(Time.deltaTime, Time.unscaledDeltaTime);
        }

#if UNITY_EDITOR
        private static void UpdateInEditor()
        {
            var time = UnityEditor.EditorApplication.timeSinceStartup;
            var dt = (float)(time - lastEditorTime);
            lastEditorTime = time;
            Dispatch(dt, dt);
        }
#endif

        private static void Dispatch(float deltaTime, float unscaledDeltaTime)
        {
            tmpReceivers.Clear();
            tmpReceivers.AddRange(receivers);
            foreach (var updateReceiver in tmpReceivers) updateReceiver(deltaTime);
            
            tmpReceivers.Clear();
            tmpReceivers.AddRange(unscaledReceivers);
            foreach (var updateReceiver in tmpReceivers) updateReceiver(unscaledDeltaTime);
        }
        
        private static bool IsRegisteredToUpdate() => Application.isPlaying ? isInstanceCreated : isAttachedToEditorUpdate;

        private static void UnregisterFromUpdate()
        {
            if (isInstanceCreated)
            {
                Destroy(instance.gameObject);
                instance = null;
                isInstanceCreated = false;
            }

            if (!isAttachedToEditorUpdate) return;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= UpdateInEditor;
            isAttachedToEditorUpdate = false;
#endif
        }

        private static void RegisterToUpdate()
        {
            UnregisterFromUpdate();
            if (Application.isPlaying)
            {
                var go = new GameObject("SequentioBarUpdateProvider")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                DontDestroyOnLoad(go);
                go.AddComponent<SequentioUpdateProvider>();
                instance = go.GetComponent<SequentioUpdateProvider>();
                isInstanceCreated = true;
                return;
            }
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += UpdateInEditor;
            isAttachedToEditorUpdate = true;
            lastEditorTime = UnityEditor.EditorApplication.timeSinceStartup;
#endif
        }
        
    }
}