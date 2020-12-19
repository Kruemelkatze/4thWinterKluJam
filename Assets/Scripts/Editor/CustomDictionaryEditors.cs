using UnityEditor;

namespace Editor
{
    public class CustomDictionaryEditors
    {
        [CustomPropertyDrawer(typeof(StringAudioDictionary))]
        //[CustomPropertyDrawer(typeof(StringStringDictionary))] // Also add here
        [CustomPropertyDrawer(typeof(CardTypeCardDataListDictionary))]
        [CustomPropertyDrawer(typeof(CardTypeGameObjectDictionary))]
        public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryPropertyDrawer
        {
        }
    }
}