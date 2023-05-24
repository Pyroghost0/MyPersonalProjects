using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SerializeField]
public class EnumDataContainer<TValue, TEnum>
{
    [SerializeField] private TValue[] content = null;
    [SerializeField] private TEnum type;

    public TValue this[int i]
    {
        get { return content[i]; }
    }

    public int Length
    {
        get { return content.Length; }
    }
}

[CustomPropertyDrawer(typeof(EnumDataContainer<,>))]
public class ItemInfoDrawer : PropertyDrawer
{
    private SerializedProperty content;
    private SerializedProperty type;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (content == null)
        {
            content = property.FindPropertyRelative("content");
        }
        if (content == null)
        {
            type = property.FindPropertyRelative("type");
        }

        float height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
        {
            if (content.arraySize != type.enumNames.Length)
            {
                content.arraySize = type.enumNames.Length;
            }
            for (int i = 0; i < content.arraySize; i++)
            {

            }
        }
        return height;
        // The 6 comes from extra spacing between the fields (2px each)
        //return EditorGUIUtility.singleLineHeight * 4 + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(position, label);

        var itemRect = new Rect(position.x, position.y + 18, position.width, 16);
        var amountRect = new Rect(position.x, position.y + 36, position.width, 16);
        //var genderRect = new Rect(position.x, position.y + 54, position.width, 16);

        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("itemType"));
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("itemAmount"));
        //EditorGUI.PropertyField(genderRect, property.FindPropertyRelative("Gender"));

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}