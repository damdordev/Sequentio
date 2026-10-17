using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(SequenceAsset))]
    public class SequenceDefinitionAssetEditor : UnityEditor.Editor
    {
        private readonly SequenceEditorHelper sequenceEditorHelper = new();
        private readonly SequenceTimelineHelper timelineHelper = new();

        private void OnDisable()
        {
            sequenceEditorHelper.Release();
        }

        public override void OnInspectorGUI()
        {
            var asset = target as SequenceAsset;
            serializedObject.Update();
            
            sequenceEditorHelper.Setup(serializedObject);
            sequenceEditorHelper.ShowSettings();
            sequenceEditorHelper.ShowStorage();

            var sequencePlan = asset.CreateDefinitionPlan();
            timelineHelper.Draw(sequencePlan, null, sequenceEditorHelper.StepList);
            
            sequenceEditorHelper.ShowSteps();
            sequenceEditorHelper.ShowSelectedStep();
            
            serializedObject.ApplyModifiedProperties();
            sequencePlan.Release();
        }
    }
}