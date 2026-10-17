using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : UnityEditor.Editor
    {
        private readonly SequenceEditorHelper sequenceEditorHelper = new();
        private readonly SequenceTimelineHelper timelineHelper = new();
        
        private void OnDisable()
        {
            sequenceEditorHelper.Release();
        }

        public override void OnInspectorGUI()
        {
            var sequencePlayer = target as SequencePlayer;
            serializedObject.Update();
            
            sequenceEditorHelper.Setup(serializedObject);
            sequenceEditorHelper.ShowPlayerControls(sequencePlayer);
            sequenceEditorHelper.ShowSettings();
            sequenceEditorHelper.ShowStorage();

            var definitionPlan = sequencePlayer.CreateDefinitionPlan();
            timelineHelper.Draw(definitionPlan, sequencePlayer, sequenceEditorHelper.StepList);
            
            sequenceEditorHelper.ShowSteps();
            sequenceEditorHelper.ShowSelectedStep();
            
            serializedObject.ApplyModifiedProperties();
            definitionPlan.Release();
            if(sequencePlayer.State == SequencePlayerState.Playing) Repaint();
        }
    }
}