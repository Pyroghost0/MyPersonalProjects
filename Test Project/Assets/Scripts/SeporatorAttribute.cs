using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Decorator drawer (used for decoration rather than a field like a float)

//Allows this to be used twice in a row
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple =true)]
public class SeporatorAttribute : PropertyAttribute
{
    public readonly float height;
    public readonly float spacing;

    public SeporatorAttribute(float height = 1f, float spacing = 10f)
    {
        this.height = height;
        this.spacing = spacing;
    }
}
