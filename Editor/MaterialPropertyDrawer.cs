using UnityEditor;
using UnityEngine;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws a material property selector using IMGUI, showing its name only for custom properties.
    /// </summary>
    public abstract class MaterialPropertyDrawer : PropertyDrawer
    {
        protected abstract int CustomTypeValue { get; }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;
            if (!property.isExpanded) return height;

            var type = property.FindPropertyRelative("type");
            height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(type, true);
            if (ShowName(type))
                height += EditorGUIUtility.standardVerticalSpacing +
                          EditorGUI.GetPropertyHeight(property.FindPropertyRelative("name"), true);
            return height;
        }

        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, label, true);
            if (property.isExpanded)
            {
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = indent + 1;
                var type = property.FindPropertyRelative("type");
                line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
                line.height = EditorGUI.GetPropertyHeight(type, true);
                EditorGUI.PropertyField(line, type, true);
                if (ShowName(type))
                {
                    var name = property.FindPropertyRelative("name");
                    line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
                    line.height = EditorGUI.GetPropertyHeight(name, true);
                    EditorGUI.PropertyField(line, name, true);
                }
                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();
        }

        private bool ShowName(SerializedProperty type) => type.hasMultipleDifferentValues || type.intValue == CustomTypeValue;
    }
}
