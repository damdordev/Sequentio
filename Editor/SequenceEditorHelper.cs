using System;
using System.Collections.Generic;
using System.Linq;
using Damdor.Vario.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws sequence settings, variables, steps, and playback controls in custom inspectors.
    /// </summary>
    public class SequenceEditorHelper
    {
        /// <summary>
        /// The sequence definition currently inspected by this helper.
        /// </summary>
        public ISequenceDefinition SequenceDefinition { get; private set; }
        /// <summary>
        /// The sequence player controlled or inspected by this helper or step.
        /// </summary>
        public ISequencePlayer SequencePlayer { get; private set; }
        /// <summary>
        /// The reorderable list used to edit and select sequence steps.
        /// </summary>
        public ReorderableList StepList { get; private set; }

        private SerializedProperty settingsProperty;
        private SerializedProperty storageProperty;
        private SerializedProperty stepsProperty;

        private readonly Dictionary<string, ReorderableList> propertyPathToReorderableList = new();
        private SerializedObject serializedObject;
        
        private GUIContent playButtonContent;
        private GUIContent stopButtonContent;
        private GUIContent pauseButtonContent;
        
        /// <summary>
        /// Binds the inspector helper to a serialized sequence asset or player.
        /// </summary>
        /// <param name="serializedObject">The serialized sequence asset or component to inspect.</param>
        public void Setup(SerializedObject serializedObject)
        {
            if (this.serializedObject == serializedObject) return;
            Release();
            this.serializedObject = serializedObject;
            SequenceDefinition = serializedObject.targetObject as ISequenceDefinition;
            SequencePlayer = serializedObject.targetObject as ISequencePlayer;
            
            var definitionProperty = serializedObject.FindProperty("definition");
            if (definitionProperty != null)
            {
                storageProperty = definitionProperty.FindPropertyRelative("storage");
                stepsProperty = definitionProperty.FindPropertyRelative("steps");

                if (!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var stepsList))
                {
                    stepsList = CreateStepsReorderableList(stepsProperty);
                }

                StepList = stepsList;
            }
            settingsProperty = serializedObject.FindProperty("settings");

            playButtonContent ??= EditorGUIUtility.IconContent("PlayButton");
            stopButtonContent ??= EditorGUIUtility.IconContent("StopButton");
            pauseButtonContent ??= EditorGUIUtility.IconContent("PauseButton On");
        }
        
        /// <summary>
        /// Clears the serialized object binding and cached references used to draw the inspector.
        /// </summary>
        public void Release()
        {
            serializedObject = null;
            SequenceDefinition = null;
            storageProperty = null;
            stepsProperty = null;
            settingsProperty = null;
            StepList = null;
        }
        
        /// <summary>
        /// Draws the playback settings of the configured serialized object.
        /// </summary>
        public void ShowSettings()
        {
            EditorGUILayout.PropertyField(settingsProperty);
        }

        /// <summary>
        /// Draws the variable storage section of the configured sequence definition.
        /// </summary>
        public void ShowStorage()
        {
            storageProperty.isExpanded = EditorGUILayout.Foldout(storageProperty.isExpanded, "Storage");
            if (storageProperty.isExpanded) EditorGUILayout.PropertyField(storageProperty);
        }

        /// <summary>
        /// Draws the reorderable list of sequence steps.
        /// </summary>
        public void ShowSteps()
        {
            StepList.DoLayoutList();
        }

        /// <summary>
        /// Draws parameter editors for the selected steps.
        /// </summary>
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
        
        /// <summary>
        /// Draws play, pause, stop, and timeline scrubbing controls for a sequence player.
        /// </summary>
        /// <param name="sequencePlayer">The player used for playback controls or progress display; null where the view supports no player.</param>
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