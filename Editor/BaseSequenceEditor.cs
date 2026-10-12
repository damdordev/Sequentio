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
                playButtonContent = EditorGUIUtility.IconContent("PlayButton");
                stopButtonContent = EditorGUIUtility.IconContent("StopButton");
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

            sequence?.Stop(StopBehaviour.ReturnToInitialState);
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
            
            stepsList.DoLayoutList();
            EditorGUILayout.BeginVertical();
            ++EditorGUI.indentLevel;
            ShowSelectedSteps(stepsList);
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
            
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

            if (GUILayout.Button(stopButtonContent)) sequence.Stop(StopBehaviour.ReturnToInitialState);
            
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

            reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
                DrawStep(property, rect, index, isActive, isFocused);

            reorderableList.elementHeightCallback += _ => 32f; 

            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, SequentioSettings.GetStepName, type =>
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

            reorderableList.multiSelect = true;
            propertyPathToReorderableList[property.propertyPath] = reorderableList;
            
            reorderableList.onSelectCallback += list => property.GetArrayElementAtIndex(list.index).isExpanded = true;
            
            return reorderableList;
        }

        private void DrawStep(SerializedProperty property, Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = property.GetArrayElementAtIndex(index);
            var step = element.managedReferenceValue as SequenceStep;

            if (step == null)
                return;

            var type = step.GetType();

            const float iconSize = 32f;
            const float valuesWidth = 210f;
            const float spacing = 6f;
            const float timeWidth = 60f;
            const float progressWidth = 100f;
            const float progressHeight = 12f;

            if (step.RunWithPreviousStep)
            {
                rect.x += 20f;
                rect.width -= 20f;
            }

            var iconPath = SequentioSettings.GetStepIcon(type);
            if (iconPath != null)
            {
                var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
                if (icon != null)
                {
                    GUI.DrawTexture(new Rect(
                            rect.x,
                            rect.y,
                            iconSize,
                            iconSize),
                        icon,
                        ScaleMode.ScaleToFit,
                        true
                    );
                }
            }

            GUI.Label(
                new Rect(
                    rect.x + iconSize,
                    rect.y,
                    valuesWidth,
                    rect.height),
                new GUIContent(
                    $"<b>{GetStepName(step)}</b>  <color=#B8B8B8>{step.TargetNameInEditor}</color>"
                ),
                new GUIStyle(EditorStyles.label)
                {
                    richText = true,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Clip
                }
            );

            GUI.Label(
                new Rect(
                    rect.xMax - timeWidth - spacing,
                    rect.y,
                    timeWidth,
                    rect.height),
                $"{step.CalculateDuration()}s",
                new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    clipping = TextClipping.Clip
                }
            );

            if (sequence != null)
            {
                EditorGUI.ProgressBar(new Rect(
                        rect.xMax - timeWidth - 2f * spacing - progressWidth,
                        rect.y + (rect.height - progressHeight) / 2f,
                        progressWidth,
                        progressHeight),
                    0.5f,
                    "");
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
                    typeof(Sequence).GetMethod("UpdateTime", BindingFlags.NonPublic | BindingFlags.Instance);
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

        private void ShowSelectedSteps(ReorderableList stepsList)
        {
            foreach (var stepIndex in stepsList.selectedIndices)
            {
                var element = stepsProperty.GetArrayElementAtIndex(stepIndex);
                var step = (SequenceStep)element.managedReferenceValue;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(
                    element,
                    new GUIContent(GetStepName(step)),
                    true
                );
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
            }
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