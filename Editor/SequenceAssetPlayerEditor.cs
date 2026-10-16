using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(SequenceAssetPlayer))]
    public class SequenceAssetPlayerEditor : UnityEditor.Editor
    {
        private SerializedProperty assetProperty;
        private SerializedProperty storageProperty;
        
        private GUIContent playButtonContent;
        private GUIContent stopButtonContent;
        private GUIContent pauseButtonContent;

        private bool attachedToUpdate;
        private SequenceAssetPlayer sequencePlayer;
        private float lastTime;
        private MethodInfo updateTimeMethod;
        private object[] updateTimeMethodParameters = new object[1];

        private void OnEnable()
        {
            assetProperty = serializedObject.FindProperty("asset");
            storageProperty = serializedObject.FindProperty("overrideStorage");
            
            playButtonContent = EditorGUIUtility.IconContent("PlayButton");
            stopButtonContent = EditorGUIUtility.IconContent("StopButton");
            pauseButtonContent = EditorGUIUtility.IconContent("PauseButton On");

            if (!attachedToUpdate)
            {
                attachedToUpdate = true;
                EditorApplication.update += EditorUpdate;
            }
        }

        private void OnDisable()
        {
            if (attachedToUpdate)
            {
                EditorApplication.update -= EditorUpdate;
                attachedToUpdate = false;
            }
        }

        public override void OnInspectorGUI()
        {
            sequencePlayer = (SequenceAssetPlayer)target;
            ShowPlayControls(sequencePlayer);
            
            EditorGUILayout.PropertyField(assetProperty);
            EditorGUILayout.PropertyField(storageProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ShowPlayControls(SequenceAssetPlayer sequencePlayer)
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (sequencePlayer.State != SequenceState.Playing)
            {
                if (GUILayout.Button(playButtonContent)) sequencePlayer.Play();
            }
            else
            {
                if (GUILayout.Button(pauseButtonContent)) sequencePlayer.Pause();
            }

            if (GUILayout.Button(stopButtonContent)) sequencePlayer.Stop(StopBehaviour.ReturnToInitialState);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            ShowProgress(sequencePlayer);
            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ShowProgress(SequenceAssetPlayer sequencePlayer)
        {
            var newTime = GUILayout.HorizontalSlider(sequencePlayer.CurrentTime, 0.0f, sequencePlayer.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequencePlayer.CurrentTime))
            {
                sequencePlayer.MoveToTime(newTime);
                sequencePlayer.Pause();
            }
        }
        
        private void EditorUpdate()
        {
            if (Application.isPlaying) return;
            var mono = (MonoBehaviour)sequencePlayer;
            if (sequencePlayer == null || mono == null || !mono.isActiveAndEnabled) return;

            if (updateTimeMethod == null)
            {
                updateTimeMethod =
                    typeof(SequenceAssetPlayer).GetMethod("UpdateTime", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            var time = Time.realtimeSinceStartup;
            if (lastTime > 0f)
            {
                var dt = time - lastTime;
                updateTimeMethodParameters[0] = dt;
                updateTimeMethod.Invoke(sequencePlayer, updateTimeMethodParameters);
            }

            lastTime = time;
            Repaint();
        }
        
    }
}