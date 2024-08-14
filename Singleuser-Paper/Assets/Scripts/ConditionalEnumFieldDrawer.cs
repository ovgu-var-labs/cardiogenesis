using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalEnumFieldAttribute))]
public class ConditionalEnumFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalEnumFieldAttribute conditional = (ConditionalEnumFieldAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(conditional.EnumFieldName);

        if (enumProperty != null && enumProperty.enumValueIndex == conditional.EnumValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalEnumFieldAttribute conditional = (ConditionalEnumFieldAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(conditional.EnumFieldName);

        if (enumProperty != null && enumProperty.enumValueIndex == conditional.EnumValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif
