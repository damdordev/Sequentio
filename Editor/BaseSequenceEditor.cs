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
    public class BaseSequenceEditor : UnityEditor.Editor
    {
        private ISequence sequence;
        private ISequenceAsset sequenceAsset;

        private SerializedProperty storageProperty;
        private SerializedProperty stepsProperty;
        private SerializedProperty settingsProperty;
        
        private GUIContent playButtonContent;
        private GUIContent stopButtonContent;
        private GUIContent pauseButtonContent;
        private float lastTime;
        private MethodInfo updateTimeMethod;
        private bool attachedToUpdate;
        
        private readonly Dictionary<string, ReorderableList> propertyPathToReorderableList = new();
        private object[] updateTimeMethodParameters = new object[1];

        private void OnEnable()
        {
            RetrieveSequenceAndProperties();
            if (sequenceAsset != null)
            {
                playButtonContent = EditorGUIUtility.IconContent("PlayButton On");
                stopButtonContent = EditorGUIUtility.IconContent("animationdopesheetkeyframe");
                pauseButtonContent = EditorGUIUtility.IconContent("PauseButton On");
                EditorApplication.update += EditorUpdate;
                attachedToUpdate = true;
                lastTime = -1f;
            }
        }

        private void OnDisable()
        {
            if (attachedToUpdate)
            {
                EditorApplication.update -= EditorUpdate;
                attachedToUpdate = true;
            }

            sequence = null;
            sequenceAsset = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if(!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var stepsList))
            {
                stepsList = CreateStepsReorderableList(stepsProperty);
            }

            var wasEnabled = GUI.enabled;
            if(sequence != null)
            {
                ShowPlayControls();
                if (sequence.State != SequenceState.Idle) GUI.enabled = false;
            }

            EditorGUILayout.PropertyField(settingsProperty);

            storageProperty.isExpanded = EditorGUILayout.Foldout(storageProperty.isExpanded, "Storage");
            if (storageProperty.isExpanded)
            {
                EditorGUILayout.PropertyField(storageProperty);
            }
            
            stepsProperty.isExpanded = EditorGUILayout.Foldout(stepsProperty.isExpanded, "Steps");
            if (stepsProperty.isExpanded) stepsList.DoLayoutList();
            
            if (sequence != null && sequence.State != SequenceState.Idle) GUI.enabled = wasEnabled;
            
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
                if (GUILayout.Button(playButtonContent)) sequence.Play();
            }
            else
            {
                if (GUILayout.Button(pauseButtonContent)) sequence.Pause();
            }

            if (GUILayout.Button(stopButtonContent)) sequence.Stop();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            ShowProgress();
            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowProgress()
        {
            var newTime = GUILayout.HorizontalSlider(sequence.CurrentTime, 0.0f, sequence.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequence.CurrentTime))
            {
                sequence.MoveToTime(newTime);
                sequence.Pause();
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
                var element = property.GetArrayElementAtIndex(index);
                var step = (SequenceStep)element.managedReferenceValue;
                var indent = step.Append ? 30f : 10f;
                EditorGUI.PropertyField(
                    new Rect(rect.x + indent, rect.y, rect.width - indent, rect.height),
                    property.GetArrayElementAtIndex(index),
                    new GUIContent(GetStepName(step)),
                    true
                );
            };
            
            reorderableList.elementHeightCallback += index 
                => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index));

            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, SequentioSettings.GetStepName, type =>
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
            var mono = (MonoBehaviour)sequence;
            if (sequence == null || mono == null || !mono.isActiveAndEnabled) return;

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
        
        private void RetrieveSequenceAndProperties()
        {
            sequence = serializedObject.targetObject as ISequence;
            sequenceAsset = serializedObject.targetObject as ISequenceAsset;
            
            storageProperty = serializedObject.FindProperty("storage");
            stepsProperty = serializedObject.FindProperty("steps");
            settingsProperty = serializedObject.FindProperty("settings");
        }

        private string GetStepName(SequenceStep step)
        {
            return SequentioSettings.GetStepName(step.GetType());
        }

    }
    
}