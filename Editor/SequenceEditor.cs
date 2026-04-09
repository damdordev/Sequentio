using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Damdor.Foundation.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(Sequence))]
    public class SequenceEditor : UnityEditor.Editor
    {
        private readonly Dictionary<string, ReorderableList> propertyPathToReorderableList = new();
        private GUIContent playButtonContent;
        private GUIContent stopButtonContent;
        private GUIContent pauseButtonContent;
        private Sequence sequence;
        private float lastTime;
        private MethodInfo updateTimeMethod;
        private object[] updateTimeMethodParameters = new object[1];

        private void OnEnable()
        {
            playButtonContent = EditorGUIUtility.IconContent("PlayButton On");
            stopButtonContent = EditorGUIUtility.IconContent("animationdopesheetkeyframe");
            pauseButtonContent = EditorGUIUtility.IconContent("PauseButton On");
            EditorApplication.update += EditorUpdate;
            lastTime = -1f;
        }

        private void OnDisable()
        {
            var sequence = (Sequence) serializedObject.targetObject;
            sequence.Stop();
            EditorApplication.update -= EditorUpdate;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            sequence = (Sequence)serializedObject.targetObject;
            
            var storageProperty = serializedObject.FindProperty("storage");
            var stepsProperty = serializedObject.FindProperty("steps");
            var settingsProperty = serializedObject.FindProperty("settings");

            if(!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var stepsList))
            {
                stepsList = CreateStepsReorderableList(stepsProperty);
            }

            ShowPlayControls();

            var wasEnabled = GUI.enabled;
            if (sequence.State != SequenceState.Idle) GUI.enabled = false;

            EditorGUILayout.PropertyField(settingsProperty);

            storageProperty.isExpanded = EditorGUILayout.Foldout(storageProperty.isExpanded, "Storage");
            if (storageProperty.isExpanded)
            {
                EditorGUILayout.PropertyField(storageProperty);
            }
            
            stepsProperty.isExpanded = EditorGUILayout.Foldout(stepsProperty.isExpanded, "Steps");
            if (stepsProperty.isExpanded) stepsList.DoLayoutList();
            
            if (sequence.State != SequenceState.Idle) GUI.enabled = wasEnabled;
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ShowPlayControls()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (sequence.State != SequenceState.Playing)
            {
                ShowPlayButton(sequence);
            }
            else
            {
                ShowPauseButton(sequence);
            }

            ShowStopButton(sequence);
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            ShowProgress(sequence);
            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowPlayButton(Sequence sequence)
        {
            if (!GUILayout.Button(playButtonContent)) return;
            sequence.Play();
        }
        
        private void ShowPauseButton(Sequence sequence)
        {
            if (!GUILayout.Button(pauseButtonContent)) return;
            sequence.Pause();
        }
        
        private void ShowStopButton(Sequence sequence)
        {
            if (!GUILayout.Button(stopButtonContent)) return;
            sequence.Stop();
        }

        private void ShowProgress(Sequence sequence)
        {
            var newTime = GUILayout.HorizontalSlider(sequence.CurrentTime, 0.0f, sequence.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequence.CurrentTime))
            {
                sequence.MoveToTime(newTime);
            }
        }
        
        private ReorderableList CreateStepsReorderableList(SerializedProperty property)
        {
            var reorderableList = new ReorderableList(
                property.serializedObject,
                property,
                true,
                true,
                true,
                true
            );

            reorderableList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Steps");
            };

            reorderableList.drawElementCallback += (rect, index, _, _) =>
            {
                EditorGUI.PropertyField(
                    new Rect(rect.x + 10f, rect.y, rect.width - 10f, rect.height),
                    property.GetArrayElementAtIndex(index),
                    true
                );
            };
            
            reorderableList.elementHeightCallback += index 
                => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index));

            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, t => t.Name, type =>
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    element.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                }).Show(new Rect(rect.x - 300, rect.y, rect.width + 300, rect.height));
            };
            
            propertyPathToReorderableList[property.propertyPath] = reorderableList;
            
            return reorderableList;
        }
        
        private void EditorUpdate()
        {
            if (Application.isPlaying) return;
            if (sequence == null || !sequence.isActiveAndEnabled) return;

            if (updateTimeMethod == null)
            {
                updateTimeMethod = typeof(Sequence).GetMethod("UpdateTime", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            
            var time = Time.realtimeSinceStartup;
            if(lastTime > 0f)
            {
                var dt = time - lastTime;
                updateTimeMethodParameters[0] = dt;
                updateTimeMethod.Invoke(sequence, updateTimeMethodParameters);
            }

            lastTime = time;
        }

    }
    
}