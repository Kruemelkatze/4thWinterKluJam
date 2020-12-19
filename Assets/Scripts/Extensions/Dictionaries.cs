using System;
using Cards;
using General;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StringAudioDictionary : SerializableDictionary<string, Audio>
{
}

[Serializable]
public class CardTypeCardDataListDictionary : SerializableDictionary<CardType, CardData[]>
{
}

[Serializable]
public class CardTypeGameObjectDictionary : SerializableDictionary<CardType, GameObject>
{
}


// public class StringStringDictionary: SerializeableDictionary<string, string> {} // Add here

// Also add to ../Editor/CustomDictionaryEditors.cs