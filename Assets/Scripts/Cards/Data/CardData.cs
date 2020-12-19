using General;
using UnityEngine;

public class CardData : ScriptableObject
{
    public string name;
    public string text;
    public Sprite icon;

    public int attack = 0;
    public int armor = 0;
    public int health = 0;

    public int pointsOnSolve = 10;

    public Audio[] narratorLines;
}