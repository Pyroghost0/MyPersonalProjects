using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(EnemyAbility))]
public class EnemyAbilityDrawer : PropertyDrawer
{
    private SerializedProperty _name;
    private SerializedProperty damage;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        Rect foldBox = new Rect(position.xMin, position.yMin, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldBox, property.isExpanded, label);//_name.stringValue + " Ability");
        if (property.isExpanded)
        {
            _name = property.FindPropertyRelative("_name");
            damage = property.FindPropertyRelative("damage");
            DrawNameProperty(position);
            DrawDamageProperty(position);
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //float lineHeight = EditorGUIUtility.singleLineHeight;
        return EditorGUIUtility.singleLineHeight * (property.isExpanded ? 3 : 1);
        //return base.GetPropertyHeight(property, label);
    }

    private void DrawNameProperty(Rect position)
    {
        EditorGUI.PropertyField(new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight, 
            position.size.x, EditorGUIUtility.singleLineHeight), _name, new GUIContent("Name"));
    }

    private void DrawDamageProperty(Rect position)
    {
        EditorGUIUtility.labelWidth = 60;
        EditorGUI.PropertyField(new Rect(position.xMin + position.width * .25f, position.yMin + EditorGUIUtility.singleLineHeight*2, 
            position.size.x * .5f, EditorGUIUtility.singleLineHeight), damage, new GUIContent("Damage"));
    }
}
