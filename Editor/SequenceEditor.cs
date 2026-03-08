using System;
using System.Collections.Generic;
using System.Linq;
using Damdor.Foundation.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    [CustomEditor(typeof(Sequence))]
    public class SequenceEditor : UnityEditor.Editor
    {
        private readonly Dictionary<string, ReorderableList> propertyPathToReorderableList = new();

        public override void OnInspectorGUI()
        {
            var storageProperty = serializedObject.FindProperty("storage");
            var stepsProperty = serializedObject.FindProperty("steps");

            if(!propertyPathToReorderableList.TryGetValue(stepsProperty.propertyPath, out var statesList))
            {
                statesList = CreateStepsReorderableList(stepsProperty);
            }
            
            EditorGUILayout.PropertyField(storageProperty);
            statesList.DoLayoutList();
        }

        private ReorderableList CreateStepsReorderableList(SerializedProperty property)
        {
            var reorderableList = new ReorderableList(
                property.serializedObject,
                property,
                true,
                true,
                true,
                true
            );

            reorderableList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Steps");
            };

            reorderableList.drawElementCallback += (rect, index, _, _) =>
            {
                EditorGUI.PropertyField(
                    new Rect(rect.x + 10f, rect.y, rect.width - 10f, rect.height),
                    property.GetArrayElementAtIndex(index),
                    true
                );
            };
            
            reorderableList.elementHeightCallback += index 
                => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index));

            reorderableList.onAddDropdownCallback += (rect, _) =>
            {
                var types = SequentioSettings.SupportedStepTypes.OrderBy(t => t.Name).ToList();
                new HierarchicalDropdown<Type>(types, t => t.Name, type =>
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    element.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                }).Show(new Rect(rect.x - 300, rect.y, rect.width + 300, rect.height));
            };
            
            propertyPathToReorderableList[property.propertyPath] = reorderableList;
            
            return reorderableList;
        }

    }
    
}