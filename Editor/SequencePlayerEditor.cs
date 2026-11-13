using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws the custom inspector and timeline for a sequence player.
    /// </summary>
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : UnityEditor.Editor
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
            var sequencePlayer = target as SequencePlayer;
            serializedObject.Update();
            
            sequenceEditorHelper.Setup(serializedObject);
            sequenceEditorHelper.ShowPlayerControls(sequencePlayer);
            var wasGuiEnabled = GUI.enabled;
            if (sequencePlayer.State != SequencePlayerState.Idle) GUI.enabled = false;
            sequenceEditorHelper.ShowSettings();
            sequenceEditorHelper.ShowStorage();

            var definitionPlan = SequenceCompiler.CreateDefinitionPlan(sequencePlayer, sequencePlayer.Settings);
            timelineHelper.Draw(definitionPlan, sequencePlayer, sequenceEditorHelper.StepList);
            
            sequenceEditorHelper.ShowSteps();
            sequenceEditorHelper.ShowSelectedStep();
            
            serializedObject.ApplyModifiedProperties();
            definitionPlan.Release();
            if(wasGuiEnabled) GUI.enabled = true;
            if(sequencePlayer.State == SequencePlayerState.Playing) Repaint();
        }
    }
}