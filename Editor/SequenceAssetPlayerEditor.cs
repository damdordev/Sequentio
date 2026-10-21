using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(SequenceAssetPlayer))]
    public sealed class SequenceAssetPlayerEditor : UnityEditor.Editor
    {
        private readonly SequenceEditorHelper sequenceEditorHelper = new();
        private readonly SequenceTimelineHelper timelineHelper = new();

        private void OnDisable()
        {
            sequenceEditorHelper.Release();
        }

        public override void OnInspectorGUI()
        {
            var player = (SequenceAssetPlayer)target;
            serializedObject.Update();
            sequenceEditorHelper.Setup(serializedObject);

            using (new EditorGUI.DisabledScope(player.State != SequencePlayerState.Idle))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("asset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("storageOverride"), true);
            }

            serializedObject.ApplyModifiedProperties();
            if (player.Asset == null) return;

            sequenceEditorHelper.ShowPlayerControls(player);
            var plan = player.CreateDefinitionPlan();
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
