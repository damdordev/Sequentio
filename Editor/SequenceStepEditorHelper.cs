using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    public static class SequenceStepEditorHelper
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

        public static void DrawQuickPreview(SerializedProperty property, ISequencePlayer sequencePlayer, Rect rect, int index)
        {
            var element = property.GetArrayElementAtIndex(index);
            if (element.managedReferenceValue is not SequenceStep step) return;

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

            ShowIcon(iconRect, step);
            ShowLabel(labelRect, step);
            ShowDuration(durationRect, step);

            if (sequencePlayer != null)
            {
                ShowProgress(processRect, sequencePlayer, step);
            }
        }

        public static string GetStepName(SequenceStep step)
        {
            return SequentioSettings.GetStepName(step.GetType());
        }

        public static void ShowIcon(Rect rect, SequenceStep step)
        {
            ShowIcon(rect, step.GetType());
        }

        public static void ShowIcon(Rect rect, Type type)
        {
            var icon = GetIcon(type);
            if (icon == null) return;

            GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit, true);
        }

        private static Texture2D GetIcon(Type type)
        {
            var iconPath = SequentioSettings.GetStepIcon(type);
            if (iconPath == null) return null;

            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            return icon;
        }

        public static void ShowLabel(Rect rect, SequenceStep step)
        {
            GUI.Label(
                rect,
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
        }

        public static void ShowDuration(Rect rect, SequenceStep step)
        {
            GUI.Label(
                rect,
                $"{step.CalculatedDuration}s",
                new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    clipping = TextClipping.Clip
                }
            );
        }

        public static void ShowStepDetails(SerializedProperty property)
        {
            var step = (SequenceStep)property.managedReferenceValue;

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
                    $"<b>{GetStepName(step)}</b>  <color=#B8B8B8>{step.TargetNameInEditor}</color>"
                ),
                titleStyle,
                GUILayout.Height(32)
            );

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(4);

            // var iterator = property.Copy();
            // var end = iterator.GetEndProperty();
            // var enterChildren = true;
            //
            // while (iterator.NextVisible(enterChildren) &&
            //        !SerializedProperty.EqualContents(iterator, end))
            // {
            //     enterChildren = false;
            //     EditorGUILayout.PropertyField(iterator, true);
            // }
            
            ShowStepFields(property);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        public static void ShowProgress(Rect rect, ISequencePlayer sequencePlayer, SequenceStep step)
        {
            EditorGUI.ProgressBar(rect, sequencePlayer.GetProgress(step), "");
        }

        public static void ShowStepFields(SerializedProperty property)
        {
            var step = property.managedReferenceValue as SequenceStep;
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