using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws an interactive sequence timeline with step selection, zooming, panning, and a playback indicator.
    /// </summary>
    public sealed class SequenceTimelineHelper
    {
        private const float RulerHeight = 24f;
        private const float RowHeight = 28f;
        private const float Padding = 8f;
        private const float MinPixelsPerSecond = 1f;
        private const float MaxPixelsPerSecond = 2000f;

        private SequencePlan sequenceDefinitionPlan;
        private int rowCount;
        private Vector2 scrollPosition;
        private float pixelsPerSecond = 100f;
        private int lastSelectedIndex = -1;
        private GUIStyle stepLabelStyle;

        /// <summary>
        /// Draws the sequence timeline and handles zooming, panning, and step selection.
        /// </summary>
        /// <param name="sequenceDefinitionPlan">The compiled sequence plan.</param>
        /// <param name="sequencePlayer">The player used for playback controls or progress display; null where the view supports no player.</param>
        /// <param name="stepsList">The optional step list synchronized with timeline selection.</param>
        public void Draw(SequencePlan sequenceDefinitionPlan, ISequencePlayer sequencePlayer, ReorderableList stepsList)
        {
            this.sequenceDefinitionPlan = sequenceDefinitionPlan;
            rowCount = sequenceDefinitionPlan.Steps.Count > 0
                ? 1 + sequenceDefinitionPlan.Steps.Max(s => s.IndexInGroup)
                : 1;
            var oldScrollPosition = scrollPosition;
            var oldPixelsPerSecond = pixelsPerSecond;
            var selectedIndex = stepsList != null ? stepsList.selectedIndices.Count > 0 ? stepsList.index : -1 : 0;

            EditorGUILayout.Space(8f);

            var horizontalScrollbarHeight = GUI.skin.horizontalScrollbar.fixedHeight;
            var verticalScrollbarWidth = GUI.skin.verticalScrollbar.fixedWidth;
            var height = Mathf.Clamp(RulerHeight + rowCount * RowHeight + Padding * 2f +
                                     horizontalScrollbarHeight, 110f, 260f);
            var rect = GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true));
            var bodyRect = new Rect(rect.x, rect.y + RulerHeight, rect.width, rect.height - RulerHeight);
            var viewportWidth = Mathf.Max(1f, bodyRect.width - verticalScrollbarWidth);
            var viewportHeight = Mathf.Max(1f, bodyRect.height - horizontalScrollbarHeight);
            var controlId = GUIUtility.GetControlID(FocusType.Passive);

            HandleInput(rect, controlId);

            var contentRect = new Rect(0f, 0f,
                Mathf.Max(viewportWidth, sequenceDefinitionPlan.Duration * pixelsPerSecond + Padding * 2f + 60f),
                Mathf.Max(viewportHeight, rowCount * RowHeight + Padding * 2f));

            if (selectedIndex != lastSelectedIndex)
            {
                RevealSelection(selectedIndex, viewportWidth, viewportHeight);
                lastSelectedIndex = selectedIndex;
            }

            scrollPosition.x = Mathf.Clamp(scrollPosition.x, 0f, contentRect.width - viewportWidth);
            scrollPosition.y = Mathf.Clamp(scrollPosition.y, 0f, contentRect.height - viewportHeight);

            EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin
                ? new Color(0.13f, 0.13f, 0.13f)
                : new Color(0.8f, 0.8f, 0.8f));

            scrollPosition = GUI.BeginScrollView(bodyRect, scrollPosition, contentRect, true, true);
            DrawGrid(viewportWidth, contentRect.height);
            var selectionChanged = DrawSteps(sequenceDefinitionPlan, stepsList, viewportWidth, viewportHeight);
            if (sequenceDefinitionPlan.Steps.Count == 0)
            {
                GUI.Label(new Rect(Padding, Padding, viewportWidth - Padding * 2f, RowHeight),
                    "No steps with a positive duration.", EditorStyles.miniLabel);
            }
            GUI.EndScrollView();

            DrawRuler(new Rect(rect.x, rect.y, viewportWidth, RulerHeight));
            DrawPlayhead(new Rect(rect.x, rect.y, viewportWidth, RulerHeight + viewportHeight), sequencePlayer);
        }

        private void HandleInput(Rect rect, int controlId)
        {
            var currentEvent = Event.current;
            switch (currentEvent.GetTypeForControl(controlId))
            {
                case EventType.ScrollWheel when GUI.enabled && rect.Contains(currentEvent.mousePosition):
                    var mouseX = currentEvent.mousePosition.x - rect.x;
                    var timeAtMouse = (scrollPosition.x + mouseX - Padding) / pixelsPerSecond;
                    pixelsPerSecond = Mathf.Clamp(pixelsPerSecond * Mathf.Exp(-currentEvent.delta.y * 0.1f),
                        MinPixelsPerSecond, MaxPixelsPerSecond);
                    scrollPosition.x = timeAtMouse * pixelsPerSecond + Padding - mouseX;
                    currentEvent.Use();
                    break;
                case EventType.MouseDown when GUI.enabled && currentEvent.button == 2 &&
                                              rect.Contains(currentEvent.mousePosition):
                    GUIUtility.hotControl = controlId;
                    currentEvent.Use();
                    break;
                case EventType.MouseDrag when GUIUtility.hotControl == controlId:
                    scrollPosition -= currentEvent.delta;
                    currentEvent.Use();
                    break;
                case EventType.MouseUp when GUIUtility.hotControl == controlId:
                    GUIUtility.hotControl = 0;
                    currentEvent.Use();
                    break;
            }
        }

        private void RevealSelection(int selectedIndex, float viewportWidth, float viewportHeight)
        {
            foreach (var stepPlan in sequenceDefinitionPlan.Steps)
            {
                if (stepPlan.OriginalIndex != selectedIndex) continue;

                var startX = Padding + (stepPlan.StartTime - stepPlan.Delay) * pixelsPerSecond;
                var endX = Padding + (stepPlan.StartTime + stepPlan.Duration) * pixelsPerSecond;
                if (startX < scrollPosition.x || endX > scrollPosition.x + viewportWidth)
                    scrollPosition.x = Mathf.Max(0f, startX - Padding);

                var startY = Padding + stepPlan.IndexInGroup * RowHeight;
                if (startY < scrollPosition.y)
                    scrollPosition.y = startY - Padding;
                else if (startY + RowHeight > scrollPosition.y + viewportHeight)
                    scrollPosition.y = startY + RowHeight + Padding - viewportHeight;
                return;
            }
        }

        private float GetTickInterval()
        {
            var minimumInterval = 70f / pixelsPerSecond;
            var magnitude = Mathf.Pow(10f, Mathf.Floor(Mathf.Log10(minimumInterval)));
            var normalized = minimumInterval / magnitude;
            return magnitude * (normalized <= 1f ? 1f : normalized <= 2f ? 2f : normalized <= 5f ? 5f : 10f);
        }

        private void DrawGrid(float viewportWidth, float height)
        {
            var interval = GetTickInterval();
            var firstTick = Mathf.Max(0f, Mathf.Floor((scrollPosition.x - Padding) / pixelsPerSecond / interval));
            var tickCount = Mathf.CeilToInt(viewportWidth / pixelsPerSecond / interval) + 2;
            for (var i = 0; i < tickCount; i++)
            {
                var x = Padding + (firstTick + i) * interval * pixelsPerSecond;
                EditorGUI.DrawRect(new Rect(x, 0f, 1f, height), new Color(0.5f, 0.5f, 0.5f, 0.25f));
            }
        }

        private void DrawRuler(Rect rect)
        {
            GUI.BeginGroup(rect);
            var interval = GetTickInterval();
            var firstTick = Mathf.Max(0f, Mathf.Floor((scrollPosition.x - Padding) / pixelsPerSecond / interval));
            var tickCount = Mathf.CeilToInt(rect.width / pixelsPerSecond / interval) + 2;
            for (var i = 0; i < tickCount; i++)
            {
                var time = (firstTick + i) * interval;
                var x = Padding + time * pixelsPerSecond - scrollPosition.x;
                GUI.Label(new Rect(x + 3f, 0f, 70f, RulerHeight - 4f),
                    time.ToString("0.###", CultureInfo.InvariantCulture) + " s", EditorStyles.miniLabel);
                EditorGUI.DrawRect(new Rect(x, RulerHeight - 5f, 1f, 5f), Color.gray);
            }
            GUI.EndGroup();
        }

        private void DrawPlayhead(Rect rect, ISequencePlayer sequencePlayer)
        {
            if (sequencePlayer == null || sequencePlayer.State == SequencePlayerState.Idle) return;

            var x = Padding + sequencePlayer.CurrentTime * pixelsPerSecond - scrollPosition.x;
            GUI.BeginGroup(rect);
            EditorGUI.DrawRect(new Rect(x - 1f, 0f, 2f, rect.height), Color.red);
            GUI.EndGroup();
        }

        private bool DrawSteps(SequencePlan sequenceDefinitionPlan, ReorderableList stepsList, float viewportWidth, float viewportHeight)
        {
            stepLabelStyle ??= new GUIStyle(EditorStyles.miniBoldLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                clipping = TextClipping.Clip,
                padding = new RectOffset(5, 5, 0, 0),
                normal = { textColor = Color.white },
                richText = true
            };

            var selectionChanged = false;
            var visibleRect = new Rect(scrollPosition.x, scrollPosition.y, viewportWidth, viewportHeight);
            foreach (var stepPlan in sequenceDefinitionPlan.Steps)
            {
                if(stepPlan.Duration <= 0f && stepPlan.Delay <= 0f) continue;
                var rect = new Rect(Padding + stepPlan.StartTime * pixelsPerSecond, Padding + stepPlan.IndexInGroup * RowHeight,
                    Mathf.Max(1f, stepPlan.Duration * pixelsPerSecond), RowHeight - 4f);
                var delayRect = new Rect(rect.x - stepPlan.Delay * pixelsPerSecond, rect.y,
                    stepPlan.Delay * pixelsPerSecond, rect.height);
                var fullRect = Rect.MinMaxRect(delayRect.xMin, rect.yMin, rect.xMax, rect.yMax);
                if (!fullRect.Overlaps(visibleRect)) continue;

                if (GUI.enabled && Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
                    fullRect.Contains(Event.current.mousePosition))
                {
                    stepsList?.Select(stepPlan.OriginalIndex);
                    stepsList?.GrabKeyboardFocus();
                    stepsList?.onSelectCallback?.Invoke(stepsList);
                    lastSelectedIndex = stepPlan.OriginalIndex;
                    selectionChanged = true;
                    Event.current.Use();
                }

                var selected = stepsList?.IsSelected(stepPlan.OriginalIndex) ?? false; 
                var name = $"<b>{StepEditorHelper.GetStepName(stepPlan.Definition)}</b> <color=#B8B8B8>{stepPlan.Definition.GetTargetNameInEditor(sequenceDefinitionPlan.Storage)}</color>";
                var tooltip = $"{name}\n" +
                              $"Start: {stepPlan.StartTime:0.###} s\nDuration: {stepPlan.Duration:0.###} s\nDelay: {stepPlan.Delay:0.###} s";
                var borderColor = selected ? new Color(0.45f, 0.8f, 1f) : new Color(0.3f, 0.4f, 0.5f);
                var fillColor = selected ? new Color(0.15f, 0.45f, 0.7f) : new Color(0.22f, 0.3f, 0.38f);

                if (stepPlan.Delay > 0f)
                {
                    var delayBorderColor = borderColor;
                    delayBorderColor.a = 0.2f;
                    var delayFillColor = fillColor;
                    delayFillColor.a = 0.2f;
                    EditorGUI.DrawRect(delayRect, delayBorderColor);
                    EditorGUI.DrawRect(new Rect(delayRect.x + 1f, delayRect.y + 1f,
                        Mathf.Max(0f, delayRect.width - 2f), delayRect.height - 2f), delayFillColor);
                    GUI.Label(delayRect, new GUIContent("", tooltip), stepLabelStyle);
                }

                EditorGUI.DrawRect(rect, borderColor);
                var innerRect = new Rect(rect.x + 1f, rect.y + 1f, Mathf.Max(0f, rect.width - 2f), rect.height - 2f);
                EditorGUI.DrawRect(innerRect, fillColor);
                GUI.Label(innerRect, new GUIContent(name, tooltip), stepLabelStyle);
            }

            return selectionChanged;
        }
    }
}
