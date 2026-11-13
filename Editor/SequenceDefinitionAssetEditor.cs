using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws the custom inspector and timeline for a sequence asset.
    /// </summary>
    [CustomEditor(typeof(SequenceAsset))]
    public class SequenceDefinitionAssetEditor : UnityEditor.Editor
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
            var asset = target as SequenceAsset;
            serializedObject.Update();
            
            sequenceEditorHelper.Setup(serializedObject);
            sequenceEditorHelper.ShowStorage();

            var settings = SequentioPooling.PopSequenceSettings();
            var sequencePlan = SequentioSettings.Compiler.CreateDefinitionPlan(asset, settings);
            timelineHelper.Draw(sequencePlan, null, sequenceEditorHelper.StepList);
            settings?.Release();
            
            sequenceEditorHelper.ShowSteps();
            sequenceEditorHelper.ShowSelectedStep();
            
            serializedObject.ApplyModifiedProperties();
            sequencePlan.Release();
        }
    }
}