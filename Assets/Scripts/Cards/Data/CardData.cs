using System;
using Extensions;
using General;
using UnityEngine;

public class CardData : ScriptableObject
{
    public new string name;
    public string text;
    public Sprite icon;

    public int attack = 0;
    public int armor = 0;
    public int health = 0;

    public int pointsOnSolve = 10;

    public Audio[] interactSounds;
    public Audio[] narratorLines;

    public Audio GetRandomNarratorLine()
    {
        if (narratorLines == null || narratorLines.Length == 0)
        {
            return null;
        }

        return narratorLines.RandomEntry();
    }

    public Audio GetRandomInteractSound()
    {
        if (interactSounds == null || interactSounds.Length == 0)
        {
            return null;
        }

        return interactSounds.RandomEntry();
    }
}