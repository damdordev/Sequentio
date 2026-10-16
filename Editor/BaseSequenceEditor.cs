using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Damdor.Vario.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    public class BaseSequenceEditor : UnityEditor.Editor
    {
        private ISequencePlayer sequencePlayer;
        private ISequenceDefinition sequenceDefinition;

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
        private readonly SequenceTimelineHelper timelineHelper = new();
        private object[] updateTimeMethodParameters = new object[1];

        private void OnEnable()
        {
            RetrieveSequenceAndProperties();
            if (sequenceDefinition != null)
            {
                playButtonContent = EditorGUIUtility.IconContent("PlayButton");
                stopButtonContent = EditorGUIUtility.IconContent("StopButton");
                pauseButtonContent = EditorGUIUtility.IconContent("PauseButton On");
                EditorApplication.update += EditorUpdate;
                attachedToUpdate = true;
                lastTime = -1f;
            }

            for (var i = 0; i < stepsProperty.arraySize; ++i)
            {
                var element = stepsProperty.GetArrayElementAtIndex(i);
                if (element.managedReferenceValue is not SequenceStep step) continue;
                step.InvalidateDuration();
            }
        }

        private void OnDisable()
        {
            if (attachedToUpdate)
            {
                EditorApplication.update -= EditorUpdate;
                attachedToUpdate = true;
            }

            sequencePlayer?.Stop(StopBehaviour.ReturnToInitialState);
            sequencePlayer = null;
            sequenceDefinition = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if(!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var stepsList))
            {
                stepsList = CreateStepsReorderableList(stepsProperty);
            }

            var wasEnabled = GUI.enabled;
            if(sequencePlayer != null)
            {
                ShowPlayControls();
                if (sequencePlayer.State != SequenceState.Idle) GUI.enabled = false;
            }

            EditorGUILayout.PropertyField(settingsProperty);

            storageProperty.isExpanded = EditorGUILayout.Foldout(storageProperty.isExpanded, "Storage");
            if (storageProperty.isExpanded)
            {
                EditorGUILayout.PropertyField(storageProperty);
            }
            
            if (timelineHelper.Draw(sequenceDefinition.Steps, sequenceDefinition.Storage, false , sequenceDefinition, stepsList)) Repaint();

            stepsList.DoLayoutList();
            EditorGUILayout.BeginVertical();
            ++EditorGUI.indentLevel;
            ShowSelectedSteps(stepsList);
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
            
            if (sequencePlayer != null && sequencePlayer.State != SequenceState.Idle) GUI.enabled = wasEnabled;
            
            serializedObject.ApplyModifiedProperties();
        }
        
        public override bool RequiresConstantRepaint()
        {
            return sequencePlayer != null && sequencePlayer.State == SequenceState.Playing;
        }

        private void ShowPlayControls()
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

            ShowProgress();
            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowProgress()
        {
            var newTime = GUILayout.HorizontalSlider(sequencePlayer.CurrentTime, 0.0f, sequencePlayer.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequencePlayer.CurrentTime))
            {
                sequencePlayer.MoveToTime(newTime);
                sequencePlayer.Pause();
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

            reorderableList.elementHeightCallback += _ => 32f; 
            reorderableList.drawElementCallback += (rect, index, _, _) => SequenceStepEditorHelper.DrawQuickPreview(property, sequencePlayer, rect, index);

            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, SequentioSettings.GetStepPath, type =>
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    var step = (SequenceStep) Activator.CreateInstance(type);
                    var resetMethod = typeof(SequenceStep).GetMethod("Reset", BindingFlags.Instance | BindingFlags.NonPublic);
                    resetMethod.Invoke(step, null);
                    element.managedReferenceValue = step;
                    property.serializedObject.ApplyModifiedProperties();
                }).Show(new Rect(rect.x - 300, rect.y, rect.width + 300, rect.height));
            };

            propertyPathToReorderableList[property.propertyPath] = reorderableList;
            
            reorderableList.onSelectCallback += list => property.GetArrayElementAtIndex(list.index).isExpanded = true;
            
            return reorderableList;
        }

        private void EditorUpdate()
        {
            if (Application.isPlaying) return;
            var mono = (MonoBehaviour)sequencePlayer;
            if (sequencePlayer == null || mono == null || !mono.isActiveAndEnabled) return;

            if (updateTimeMethod == null)
            {
                updateTimeMethod =
                    typeof(SequencePlayer).GetMethod("UpdateTime", BindingFlags.NonPublic | BindingFlags.Instance);
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

        private void ShowSelectedSteps(ReorderableList stepsList)
        {
            foreach (var stepIndex in stepsList.selectedIndices)
            {
                if(stepIndex < 0 || stepIndex >= stepsProperty.arraySize) continue;
                SequenceStepEditorHelper.ShowStepDetails(stepsProperty.GetArrayElementAtIndex(stepIndex));
            }
        }
        
        private void RetrieveSequenceAndProperties()
        {
            sequencePlayer = serializedObject.targetObject as ISequencePlayer;
            sequenceDefinition = serializedObject.targetObject as ISequenceDefinition;
            
            storageProperty = serializedObject.FindProperty("storage");
            stepsProperty = serializedObject.FindProperty("steps");
            settingsProperty = serializedObject.FindProperty("settings");
        }

    }
    
}
