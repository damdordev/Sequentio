using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws the custom inspector and timeline for a sequence asset player.
    /// </summary>
    [CustomEditor(typeof(SequenceAssetPlayer))]
    public sealed class SequenceAssetPlayerEditor : UnityEditor.Editor
    {
        private readonly SequenceEditorHelper sequenceEditorHelper = new();
        private readonly SequenceTimelineHelper timelineHelper = new();

        private void OnDisable()
        {
            sequenceEditorHelper.Release();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            var player = (SequenceAssetPlayer)target;
            serializedObject.Update();
            sequenceEditorHelper.Setup(serializedObject);

            sequenceEditorHelper.ShowPlayerControls(player);
            using (new EditorGUI.DisabledScope(player.State != SequencePlayerState.Idle))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("asset"));
                sequenceEditorHelper.ShowSettings();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("storageOverride"), true);
            }

            serializedObject.ApplyModifiedProperties();
            if (player.Asset == null) return;
            
            var plan = SequentioSettings.Compiler.CreateDefinitionPlan(player.Asset, player.Settings);
            try
            {
                timelineHelper.Draw(plan, player, null);
            }
            finally
            {
                plan.Release();
            }

            if (player.State == SequencePlayerState.Playing) Repaint();
        }
    }
}
