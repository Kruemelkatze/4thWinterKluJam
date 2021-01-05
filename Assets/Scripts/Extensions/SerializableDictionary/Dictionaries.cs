using System;
using Cards;
using General;
using UnityEditor;
using UnityEngine;

namespace Extensions.SerializableDictionary
{
    /*
     * Add serializable dictionary types below. Also do not forget to add a CustomPropertyDrawer attribute below too.
    */

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

    // [Serializable]
    // public class StringStringDictionary: SerializeableDictionary<string, string> {} // Add here


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StringAudioDictionary))]
    [CustomPropertyDrawer(typeof(CardTypeCardDataListDictionary))]
    [CustomPropertyDrawer(typeof(CardTypeGameObjectDictionary))]
    //[CustomPropertyDrawer(typeof(StringStringDictionary))] // Also add here
    public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryPropertyDrawer
    {
    }
#endif
}