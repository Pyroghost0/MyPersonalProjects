using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;//Why this needs to be in the Editor folder (Editor folder can be inside a folder inside assets)

[CustomPropertyDrawer(typeof(SeporatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        SeporatorAttribute seporatorAttribute = attribute as SeporatorAttribute;
        Rect seporRact = new Rect(position.xMin + position.width * .1f, position.yMin + seporatorAttribute.spacing, position.width * .8f, seporatorAttribute.height);
        EditorGUI.DrawRect(seporRact, Color.white);
    }

    //If you need more than 1 line
    public override float GetHeight()
    {
        SeporatorAttribute seporatorAttribute = attribute as SeporatorAttribute;
        return seporatorAttribute.spacing *2 + seporatorAttribute.height;
    }
}
