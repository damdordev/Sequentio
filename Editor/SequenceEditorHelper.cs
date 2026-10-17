using System;
using System.Collections.Generic;
using System.Linq;
using Damdor.Vario.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    public class SequenceEditorHelper
    {
        public ISequenceDefinition SequenceDefinition { get; private set; }
        public ISequencePlayer SequencePlayer { get; private set; }
        public ReorderableList StepList { get; private set; }

        private SerializedProperty settingsProperty;
        private SerializedProperty storageProperty;
        private SerializedProperty stepsProperty;

        private readonly Dictionary<string, ReorderableList> propertyPathToReorderableList = new();
        private SerializedObject serializedObject;
        
        private GUIContent playButtonContent;
        private GUIContent stopButtonContent;
        private GUIContent pauseButtonContent;
        
        public void Setup(SerializedObject serializedObject)
        {
            if (this.serializedObject == serializedObject) return;
            Release();
            this.serializedObject = serializedObject;
            SequenceDefinition = serializedObject.targetObject as ISequenceDefinition;
            SequencePlayer = serializedObject.targetObject as ISequencePlayer;
            
            var definitionProperty = serializedObject.FindProperty("definition");
            storageProperty = definitionProperty.FindPropertyRelative("storage");
            stepsProperty = definitionProperty.FindPropertyRelative("steps");
            settingsProperty = definitionProperty.FindPropertyRelative("settings");
            
            if(!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var stepsList))
            {
                stepsList = CreateStepsReorderableList(stepsProperty);
            }
            StepList = stepsList;
            
            playButtonContent ??= EditorGUIUtility.IconContent("PlayButton");
            stopButtonContent ??= EditorGUIUtility.IconContent("StopButton");
            pauseButtonContent ??= EditorGUIUtility.IconContent("PauseButton On");
        }
        
        public void Release()
        {
            serializedObject = null;
            SequenceDefinition = null;
            storageProperty = null;
            stepsProperty = null;
            settingsProperty = null;
            StepList = null;
        }
        
        public void ShowSettings()
        {
            EditorGUILayout.PropertyField(settingsProperty);
        }

        public void ShowStorage()
        {
            storageProperty.isExpanded = EditorGUILayout.Foldout(storageProperty.isExpanded, "Storage");
            if (storageProperty.isExpanded) EditorGUILayout.PropertyField(storageProperty);
        }

        public void ShowSteps()
        {
            StepList.DoLayoutList();
        }

        public void ShowSelectedStep()
        {
            EditorGUILayout.BeginVertical();
            ++EditorGUI.indentLevel;
            
            foreach (var stepIndex in StepList.selectedIndices)
            {
                if(stepIndex < 0 || stepIndex >= stepsProperty.arraySize) continue;
                StepEditorHelper.ShowStepDetails(stepsProperty.GetArrayElementAtIndex(stepIndex), SequenceDefinition.Storage);
            }
            
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }
        
        public void ShowPlayerControls(ISequencePlayer sequencePlayer)
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (sequencePlayer.State != SequencePlayerState.Playing)
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
            var newTime = GUILayout.HorizontalSlider(sequencePlayer.CurrentTime, 0.0f, sequencePlayer.Duration, GUILayout.ExpandWidth(true));
            if (!Mathf.Approximately(newTime, sequencePlayer.CurrentTime))
            {
                sequencePlayer.MoveToTime(newTime);
                sequencePlayer.Pause();
            }

            GUILayout.Space(50f);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
        
        private ReorderableList CreateStepsReorderableList(SerializedProperty property)
        {
            var reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true);
            
            reorderableList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Steps");
            reorderableList.elementHeightCallback += _ => 32f; 
            
            reorderableList.drawElementCallback += (rect, index, _, _) => StepEditorHelper.DrawQuickPreview(property, rect, index, SequenceDefinition.Storage, SequencePlayer);
            
            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, SequentioSettings.GetStepPath, type =>
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    var step = (StepDefinition) Activator.CreateInstance(type);
                    step.Reset();
                    element.managedReferenceValue = step;
                    property.serializedObject.ApplyModifiedProperties();
                }).Show(new Rect(rect.x - 300, rect.y, rect.width + 300, rect.height));
            };
            
            propertyPathToReorderableList[property.propertyPath] = reorderableList;
            
            reorderableList.onSelectCallback += list => property.GetArrayElementAtIndex(list.index).isExpanded = true;
            
            return reorderableList;
        }
    }
}