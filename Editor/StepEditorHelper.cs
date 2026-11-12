using System;
using System.Collections.Generic;
using System.Reflection;
using Damdor.Vario;
using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws step previews, progress, and grouped parameter fields in sequence inspectors.
    /// </summary>
    public static class StepEditorHelper
    {
        private const float IconSize = 32f;
        private const float DurationWidth = 60f;
        private const float LabelWidth = 210f;
        private const float ProgressWidth = 100f;
        private const float ProgressHeight = 12f;
        private const float Spacing = 6f;
        
        private sealed class StepParameterEntry
        {
            public SerializedProperty Property;
            public int Priority;
            public int Order;
        }
        
        private sealed class StepParameterGroup
        {
            public string Name;
            public readonly List<StepParameterEntry> Entries = new();
        }

        /// <summary>
        /// Draws a step icon, label, duration, and optional playback progress in a list row.
        /// </summary>
        /// <param name="property">The serialized step or endpoint property to draw.</param>
        /// <param name="rect">The rectangle in which to draw the preview or progress.</param>
        /// <param name="index">The zero-based index of the step in the serialized array.</param>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        /// <param name="sequencePlayer">The player used for playback controls or progress display; null where the view supports no player.</param>
        public static void DrawQuickPreview(SerializedProperty property, Rect rect, int index, VarioStorage storage, ISequencePlayer sequencePlayer)
        {
            var element = property.GetArrayElementAtIndex(index);
            if (element.managedReferenceValue is not StepDefinition step) return;

            if (step.RunWithPreviousStep)
            {
                rect.x += 20f;
                rect.width -= 20f;
            }

            var iconRect = new Rect(rect.x, rect.y, IconSize, IconSize);
            var labelRect = new Rect(rect.x + IconSize, rect.y, LabelWidth, rect.height);
            var durationRect = new Rect(rect.xMax - DurationWidth - Spacing, rect.y, DurationWidth, rect.height);
            var processRect = new Rect(rect.xMax - DurationWidth - 2f * Spacing - ProgressWidth,
                rect.y + (rect.height - ProgressHeight) / 2f, ProgressWidth, ProgressHeight);

            ShowIcon(iconRect, step.GetType());
            ShowLabel(labelRect, step);
            ShowDuration(durationRect, step, storage);

            if (sequencePlayer != null) ShowProgress(processRect, sequencePlayer, step);
        }
        
        private static void ShowIcon(Rect rect, Type type)
        {
            var icon = GetIcon(type);
            if (icon == null) return;
        
            GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit, true);
        }

        /// <summary>
        /// Gets the registered editor display name of a step.
        /// </summary>
        /// <param name="step">The step definition whose progress or metadata is requested.</param>
        /// <returns>The registered display name, or an empty string if the type is not registered.</returns>
        public static string GetStepName(StepDefinition step)
        {
            return SequentioSettings.GetStepName(step.GetType());
        }

        private static Texture2D GetIcon(Type type)
        {
            var iconPath = SequentioSettings.GetStepIcon(type);
            if (iconPath == null) return null;
        
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            return icon;
        }
        
        private static void ShowLabel(Rect rect, StepDefinition step)
        {
            GUI.Label(
                rect,
                new GUIContent(
                    //$"<b>{GetStepName(step)}</b>  <color=#B8B8B8>{step.TargetNameInEditor}</color>"
                    step.GetType().Name
                ),
                new GUIStyle(EditorStyles.label)
                {
                    richText = true,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Clip
                }
            );
        }
        //
        private static void ShowDuration(Rect rect, StepDefinition step, VarioStorage storage)
        {
            GUI.Label(
                rect,
                $"{step.CalculateDuration(storage)}s",
                new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    clipping = TextClipping.Clip
                }
            );
        }
        
        /// <summary>
        /// Draws the selected step header and grouped parameter fields.
        /// </summary>
        /// <param name="property">The serialized step or endpoint property to draw.</param>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        public static void ShowStepDetails(SerializedProperty property, VarioStorage storage)
        {
            var step = (StepDefinition)property.managedReferenceValue;
        
            EditorGUILayout.BeginVertical(GUI.skin.box);
        
            EditorGUILayout.BeginHorizontal();
        
            var icon = GetIcon(step.GetType());
            if (icon != null)
            {
                GUILayout.Label(icon, GUILayout.Width(IconSize), GUILayout.Height(IconSize));
            }
            else
            {
                GUILayout.Label("", GUILayout.Width(IconSize), GUILayout.Height(IconSize));
            }
        
            var titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 18,
                alignment = TextAnchor.MiddleLeft,
                richText = true
            };
        
            GUILayout.Label(
                new GUIContent(
                    $"<b>{GetStepName(step)}</b>  <color=#B8B8B8>{step.GetTargetNameInEditor(storage)}</color>"
                ),
                titleStyle,
                GUILayout.Height(32)
            );
        
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(4);
            
            ShowStepFields(property);
        
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        
        /// <summary>
        /// Draws the current progress of a step as an editor progress bar.
        /// </summary>
        /// <param name="rect">The rectangle in which to draw the preview or progress.</param>
        /// <param name="sequencePlayer">The player used for playback controls or progress display; null where the view supports no player.</param>
        /// <param name="step">The step definition whose progress or metadata is requested.</param>
        public static void ShowProgress(Rect rect, ISequencePlayer sequencePlayer, StepDefinition step)
        {
            EditorGUI.ProgressBar(rect, sequencePlayer.GetProgress(step), "");
        }
        
        /// <summary>
        /// Draws visible serialized step fields grouped and ordered by their parameter group attributes.
        /// </summary>
        /// <param name="property">The serialized step or endpoint property to draw.</param>
        public static void ShowStepFields(SerializedProperty property)
        {
            var step = property.managedReferenceValue as StepDefinition;
            if (step == null)
                return;
        
            var groups = new List<StepParameterGroup>();
        
            var iterator = property.Copy();
            var end = iterator.GetEndProperty();
            var enterChildren = true;
            var order = 0;
        
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                enterChildren = false;
        
                var field = FindStepField(step.GetType(), iterator.name);
                var attribute = field == null
                    ? null
                    : field.GetCustomAttribute<SequenceStepParameterGroupAttribute>(true);
        
                var groupName = attribute?.ParameterGroup ?? SequenceStepParameterGroups.Default;
        
                var group = FindOrCreateGroup(groups, groupName);
        
                group.Entries.Add(new StepParameterEntry
                {
                    Property = iterator.Copy(),
                    Priority = attribute?.PriorityInGroup ?? 0,
                    Order = order++
                });
            }
        
            EditorGUILayout.BeginVertical(GUI.skin.box);
        
            foreach (var group in groups)
            {
                group.Entries.Sort((a, b) =>
                {
                    var priorityComparison = a.Priority.CompareTo(b.Priority);
                    return priorityComparison != 0
                        ? priorityComparison
                        : a.Order.CompareTo(b.Order);
                });
        
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
                if (!string.IsNullOrEmpty(group.Name))
                    EditorGUILayout.LabelField(group.Name, EditorStyles.boldLabel);
        
                foreach (var entry in group.Entries)
                    EditorGUILayout.PropertyField(entry.Property, true);
        
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }
        
            EditorGUILayout.EndVertical();
        }
        
        private static StepParameterGroup FindOrCreateGroup(List<StepParameterGroup> groups, string groupName)
        {
            foreach (var group in groups)
            {
                if (group.Name == groupName)
                    return group;
            }
        
            var newGroup = new StepParameterGroup { Name = groupName };
            groups.Add(newGroup);
            return newGroup;
        }
        
        private static FieldInfo FindStepField(Type type, string fieldName)
        {
            const BindingFlags flags =
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly;
        
            while (type != null)
            {
                var field = type.GetField(fieldName, flags);
                if (field != null)
                    return field;
        
                type = type.BaseType;
            }
        
            return null;
        }

    }
}