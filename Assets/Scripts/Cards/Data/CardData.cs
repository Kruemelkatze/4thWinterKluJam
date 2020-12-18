using General;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public string name;
    public string text;
    public Sprite icon;

    public int pointsOnSolve = 10;

    public Audio[] narratorLines;
}