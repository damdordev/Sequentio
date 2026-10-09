using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    [CustomPropertyDrawer(typeof(TweenEndpoint<,>))]
    public class TweenPointPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueSourceProperty = property.FindPropertyRelative("valueSource");
            var valueProperty = property.FindPropertyRelative("value");
            var otherTargetProperty = property.FindPropertyRelative("otherTarget");

            if (!property.isExpanded) return EditorGUI.GetPropertyHeight(property, false);
            
            var source = (TweenPointSource) valueSourceProperty.enumValueIndex;
            return source switch
            {
                TweenPointSource.ExplicitValue
                    => EditorGUI.GetPropertyHeight(valueSourceProperty) +
                       EditorGUI.GetPropertyHeight(valueProperty) + 
                       2f * EditorGUIUtility.standardVerticalSpacing + 
                       EditorGUI.GetPropertyHeight(property, false),
                TweenPointSource.CurrentValue 
                    =>  EditorGUI.GetPropertyHeight(valueSourceProperty) + 
                        EditorGUI.GetPropertyHeight(valueProperty) + 
                        2f * EditorGUIUtility.standardVerticalSpacing + 
                        EditorGUI.GetPropertyHeight(property, false),
                TweenPointSource.ReferenceTarget
                    => EditorGUI.GetPropertyHeight(valueSourceProperty) +  
                       EditorGUI.GetPropertyHeight(otherTargetProperty) +
                       EditorGUI.GetPropertyHeight(valueProperty) +
                       3f * EditorGUIUtility.standardVerticalSpacing + 
                       EditorGUI.GetPropertyHeight(property, false),
                _ => 0f
            };
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            var valueSourceProperty = property.FindPropertyRelative("valueSource");
            var valueProperty = property.FindPropertyRelative("value");
            var otherTargetProperty = property.FindPropertyRelative("otherTarget");

            var x = 0f;
            var y = position.y;
            
            EditorGUI.PropertyField(
                new Rect(position.x + x, position.y, position.width - x, EditorGUI.GetPropertyHeight(valueSourceProperty)),
                property,
                false
            );

            if (!property.isExpanded) return;
            
            x += 20f;
            y += EditorGUI.GetPropertyHeight(valueSourceProperty);
            y += EditorGUIUtility.standardVerticalSpacing;
            
            var source = (TweenPointSource) valueSourceProperty.enumValueIndex;
            
            EditorGUI.PropertyField(
                new Rect(position.x + x, y, position.width - x, EditorGUI.GetPropertyHeight(valueSourceProperty)),
                valueSourceProperty,
                new GUIContent("Source")
            );
            y += EditorGUI.GetPropertyHeight(valueSourceProperty);
            y += EditorGUIUtility.standardVerticalSpacing;
            
            switch (source)
            {
                case TweenPointSource.ExplicitValue:
                    EditorGUI.PropertyField(
                        new Rect(position.x + x, y, position.width - x, EditorGUI.GetPropertyHeight(valueProperty)),
                        valueProperty,
                        new GUIContent("Value")
                    );
                    break;
                case TweenPointSource.CurrentValue:
                    EditorGUI.PropertyField(
                        new Rect(position.x + x, y, position.width - x, EditorGUI.GetPropertyHeight(valueProperty)),
                        valueProperty,
                        new GUIContent("Shift")
                    );
                    break;
                case TweenPointSource.ReferenceTarget:
                    EditorGUI.PropertyField(
                        new Rect(position.x + x, y, position.width - x, EditorGUI.GetPropertyHeight(otherTargetProperty)),
                        otherTargetProperty,
                        new GUIContent("Other target")
                    );
                    y += EditorGUI.GetPropertyHeight(otherTargetProperty);
                    y += EditorGUIUtility.standardVerticalSpacing;
                    
                    EditorGUI.PropertyField(
                        new Rect(position.x + x, y, position.width - x, EditorGUI.GetPropertyHeight(valueProperty)),
                        valueProperty,
                        new GUIContent("Shift")
                    );
                    break;
            }
            
            x -= 20f;
            
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}