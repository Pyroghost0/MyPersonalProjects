/* Caleb Kahn
 * Assignment 6
 * Allows things to have a random letter assigned to its text box
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextElements: MonoBehaviour, ChangesTextLetter
{
    public string letter;
    readonly private string[] alphabet = new string[26] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    public Text text;

    protected virtual void Awake()
    {
        letter = string.Empty;
    }

    public virtual void NewLetter()
    {
        letter = alphabet[Random.Range(0, 26)];
        text.text = letter.ToUpper();
    }
}
