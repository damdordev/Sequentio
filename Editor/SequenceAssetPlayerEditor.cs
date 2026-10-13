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
        private SequenceAssetPlayer sequence;
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

                EditorApplication.update += EditorUpdate;
            }
        }

        private void OnDisable()
        {
            if (attachedToUpdate)
            {
                EditorApplication.update -= EditorUpdate;
                attachedToUpdate = true;
            }
        }

        public override void OnInspectorGUI()
        {
            sequence = (SequenceAssetPlayer)target;
            ShowPlayControls(sequence);
            
            EditorGUILayout.PropertyField(assetProperty);
            EditorGUILayout.PropertyField(storageProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ShowPlayControls(SequenceAssetPlayer sequence)
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (sequence.State != SequenceState.Playing)
            {
                if (GUILayout.Button(playButtonContent)) sequence.Play();
            }
            else
            {
                if (GUILayout.Button(pauseButtonContent)) sequence.Pause();
            }

            if (GUILayout.Button(stopButtonContent)) sequence.Stop(StopBehaviour.ReturnToInitialState);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            ShowProgress(sequence);
            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ShowProgress(SequenceAssetPlayer sequence)
        {
            var newTime = GUILayout.HorizontalSlider(sequence.CurrentTime, 0.0f, sequence.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequence.CurrentTime))
            {
                sequence.MoveToTime(newTime);
                sequence.Pause();
            }
        }
        
        private void EditorUpdate()
        {
            if (Application.isPlaying) return;
            var mono = (MonoBehaviour)sequence;
            if (sequence == null || mono == null || !mono.isActiveAndEnabled) return;

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
                updateTimeMethod.Invoke(sequence, updateTimeMethodParameters);
            }

            lastTime = time;
            Repaint();
        }
        
    }
}