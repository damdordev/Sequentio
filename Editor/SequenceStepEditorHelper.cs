using System;
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
        
        public static void DrawQuickPreview(SerializedProperty property, ISequence sequence, Rect rect, int index)
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
            var processRect = new Rect(rect.xMax - DurationWidth - 2f * Spacing - ProgressWidth, rect.y + (rect.height - ProgressHeight) / 2f, ProgressWidth, ProgressHeight);
            
            ShowIcon(iconRect, step);
            ShowLabel(labelRect, step);
            ShowDuration(durationRect, step);

            if (sequence != null)
            {
                ShowProgress(processRect, sequence, index);
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
            var iconPath = SequentioSettings.GetStepIcon(type);
            if (iconPath == null) return;
            
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            if (icon == null) return;

            GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit, true);
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
                $"{step.CalculateDuration()}s",
                new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    clipping = TextClipping.Clip
                }
            );
        }

        public static void ShowProgress(Rect rect, ISequence sequence, int stepNumber)
        {
            EditorGUI.ProgressBar(rect, sequence.GetProgress(stepNumber), "");
        }

    }
}