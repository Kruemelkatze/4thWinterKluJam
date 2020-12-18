using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public string name;
    public string text;
    public Sprite icon;

    public AudioClip narratorLines;
}