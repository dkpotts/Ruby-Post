using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterDialog
{
    [SerializeField]
    public string characterName;
    public Sprite dialogSprite;
    [TextArea (3,11)]
    public string[] sentences;
    public bool convoStarted;
    public bool convoPaused;
}
