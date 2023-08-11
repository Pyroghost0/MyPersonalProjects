using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataAttributes : Editor
{
    private bool oldGUI;

    private SerializedProperty _name;
    private SerializedProperty enemyType;
    private SerializedProperty attckingEnemy;
    private SerializedProperty damage;
    private SerializedProperty health;
    private SerializedProperty speed;
    private SerializedProperty playerSightRange;
    private SerializedProperty description;
    private SerializedProperty abilities;

    public delegate float UnnessiaryDeligateDifficultyCalc(int health, int speed, int damage, float range);

    private void OnEnable()
    {
        //EnemyData data = (EnemyData)target;
        //_name.stringValue = data.name;
        _name = serializedObject.FindProperty("privateName");//This is not _name (it can be private vars)
        enemyType = serializedObject.FindProperty("enemyType");
        attckingEnemy = serializedObject.FindProperty("attackingEnemy");
        damage = serializedObject.FindProperty("damage");
        health = serializedObject.FindProperty("health");
        speed = serializedObject.FindProperty("speed");
        playerSightRange = serializedObject.FindProperty("playerSightRangeTOOLTIP");
        description = serializedObject.FindProperty("descriptionTextArea");
        abilities = serializedObject.FindProperty("enemyAbilities");
    }

    public override void OnInspectorGUI()
    {
        EnemyData data = (EnemyData)target;
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(data._name.ToUpper(), EditorStyles.boldLabel);

        //float difficulty = (data.damage + data.speed) / 100f;
        UnnessiaryDeligateDifficultyCalc handler = Enemy.Difficulty;
        float difficulty = handler(health.intValue, speed.intValue, damage.intValue, playerSightRange.floatValue);
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, difficulty, "Est. Difficulty");

        oldGUI = EditorGUILayout.Toggle("Show Old GUI", oldGUI);
        if (oldGUI)
        {
            base.OnInspectorGUI();
        }
        else
        {
            serializedObject.UpdateIfRequiredOrScript();//The proper way to do stuff apparently
            EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);
            //EditorGUILayout.PropertyField(_name, new GUIContent("Name"));
            _name.stringValue = EditorGUILayout.TextField("Name", _name.stringValue);
            EditorGUILayout.PropertyField(enemyType, new GUIContent("Enemy Type"));

            EditorGUILayout.PropertyField(attckingEnemy, new GUIContent("Attcking Enemy"));
            if (attckingEnemy.boolValue)
            {
                EditorGUI.indentLevel++;
                /*EditorGUILayout.LabelField("Damage");
                damage.intValue = EditorGUILayout.IntSlider(damage.intValue, 0, 100);
                EditorGUILayout.LabelField("Player Sight Range");
                playerSightRange.floatValue = EditorGUILayout.Slider(playerSightRange.floatValue, 0f, 10f);*/

                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 70;
                EditorGUILayout.PropertyField(damage, new GUIContent("Damage"));
                playerSightRange.floatValue = EditorGUILayout.FloatField("Range", playerSightRange.floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.LabelField("Health");
            health.intValue = EditorGUILayout.IntSlider(health.intValue, 0, 100);
            EditorGUILayout.LabelField("Speed");
            speed.intValue = EditorGUILayout.IntSlider(speed.intValue, 0, 100);
            EditorGUILayout.PropertyField(abilities, new GUIContent("Enemy Abilities"));


            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Other Stuff", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(description, new GUIContent("Description"));

            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Randomize Damage"))
        {
            data.damage = Random.Range(0, 101);
        }

        if (data._name == string.Empty)
        {
            EditorGUILayout.HelpBox("Caution: No name specified!", MessageType.Warning);
        }
    }
}
