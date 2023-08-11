using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemySelectWindow : EditorWindow
{
    private enemyType selectedEnemyType = enemyType.Empty;
    private int num = 0;
    private GameObject[] selectedObjects;

    private Vector2 scrollPos = Vector2.zero;
    private Vector2 scrollPos1 = Vector2.zero;
    private Vector2 scrollPos2 = Vector2.zero;

    [MenuItem("Window/Enemy Selector")]
    public static void ShowWindow()
    {
        EnemySelectWindow e = GetWindow<EnemySelectWindow>("Enemy Selector");
        e.minSize = new Vector2(100f, 100f);
        e.maxSize = new Vector2(500f, 500f);
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
            float width = Mathf.Min(position.width, 100f);
            GUI.DrawTexture(new Rect((position.width - width) / 2, 10, width, width), Resources.Load<Sprite>("EnemySkull").texture);//Needs to be in resources folder
            EditorGUILayout.Space(10 + width);
            GUILayout.Label("Selection Filters:", EditorStyles.boldLabel);
            //Need to cast for enums
            selectedEnemyType = (enemyType)(EditorGUILayout.EnumPopup("Select Enemy Type:", selectedEnemyType));
            EditorGUILayout.Space(5);
            if (GUILayout.Button("Select All"))
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> finalSelection = new List<GameObject>();
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Enemy>().enemyData.enemyType == selectedEnemyType)
                    {
                        finalSelection.Add(enemy);
                    }
                }
                selectedObjects = finalSelection.ToArray();
                num = selectedObjects.Length;
                Selection.objects = selectedObjects;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Cycle Selection:");
            if (GUILayout.Button("Previous") && selectedObjects.Length > 1)
            {
                num--;
                if (num < 0)
                {
                    num = selectedObjects.Length - 1;
                }
                Selection.objects = new GameObject[] { selectedObjects[num] };
            }
            if (GUILayout.Button("Next") && selectedObjects.Length > 1)
            {
                num++;
                if (num >= selectedObjects.Length)
                {
                    num = 0;
                }
                Selection.objects = new GameObject[] { selectedObjects[num] };
            }
            GUILayout.EndHorizontal();

            //Horizontal Scroll
            scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1, GUILayout.Height(50));
            EditorGUILayout.TextArea("gfevakugvefvsfvbflasbhfydsvblasuviakysfbkuavyudksvf" + 
                "asuk,fdv,asvfasukdvafsk,avdsfmsavfkusvgfuksdavkudsvkuasdvafsdukkudfsav", GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();

            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(150));
            for (int i = 0; i < 15; i++)
            {
                GUILayout.Label("Scroll");
            }
            EditorGUILayout.EndScrollView();
        EditorGUILayout.EndScrollView();
    }
}
