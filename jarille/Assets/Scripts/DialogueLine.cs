using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    public Sprite characterSprite;

    [TextArea]
    public string text;
}