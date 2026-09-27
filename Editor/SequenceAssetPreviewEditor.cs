using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(SequenceAssetPreview))]
    public class SequenceAssetPreviewEditor : UnityEditor.Editor
    {
        private SerializedProperty assetProperty;
        private SerializedProperty storageProperty;

        private void OnEnable()
        {
            assetProperty = serializedObject.FindProperty("asset");
            storageProperty = serializedObject.FindProperty("storage");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(assetProperty);
            EditorGUILayout.PropertyField(storageProperty);

            if (GUILayout.Button("Generate")) GenerateSequence();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateSequence()
        {
            var preview = target as SequenceAssetPreview;
            var sequence = preview.GetComponent<Sequence>();
            if (sequence == null) sequence = preview.gameObject.AddComponent<Sequence>();

            var asset = (SequenceAsset) assetProperty.boxedValue;
            if (asset == null) sequence.Clear();
            else
            {
                sequence.CopyFrom(asset);
                sequence.Storage.CopyFrom(preview.Storage);
            }
        }
        
    }
}