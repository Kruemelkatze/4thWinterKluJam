using UnityEngine;

namespace General
{
    [CreateAssetMenu(fileName = "New Audio", menuName = "Template/Audio")]
    public class Audio : ScriptableObject
    {
        public AudioClip AudioClip => RandomEntry() ?? audioClip;

        [Tooltip(
            "If you only have a single audio clip, place it here. You can add variety by changing the Pitch Variation below.")]
        public AudioClip audioClip;

        [Tooltip(
            "If you have multiple variations of a single audio asset, add all of them here. The Audio Clip above will be ignored.")]
        public AudioClip[] audioClips;

        public string subtitle;

        public bool uiSound = false;
        public bool loop = false;
        [Range(0f, 3f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        [Range(0, 1f)] public float volumeVariation = 0f;
        [Range(0, 1f)] public float pitchVariation = 0f;

        private AudioClip RandomEntry()
        {
            return audioClips != null && audioClips.Length > 0
                ? audioClips[Random.Range(0, audioClips.Length)]
                : default;
        }
    }
}