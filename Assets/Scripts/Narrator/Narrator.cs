using System.Collections;
using General;
using UnityEditor;
using UnityEngine;

namespace Narrator
{
    public class Narrator : Singleton<Narrator>
    {
        [SerializeField] private NarratorUI narratorUI;
        [SerializeField] private float audioLengthOffset = -0.6f;

        [SerializeField] private Audio currentLine;

        [Header("Testing only")] [SerializeField]
        private Audio testAudio;

        [SerializeField] private CardData testCardData;

        public bool Busy => currentLine != null;
        public Audio CurrentLine => currentLine;

        private void Start()
        {
            ResetCurrentLine();
        }

        public bool PlayLine(CardData cardData)
        {
            if (!cardData)
                return false;

            var randomLine = cardData.GetRandomNarratorLine();
            return PlayLine(randomLine);
        }

        public bool PlayLine(Audio line)
        {
            if (Busy || line == null)
                return false;

            if (line.AudioClip == null)
            {
                Debug.LogWarning("No Audioclip on Audio asset: " + line.name);
                return false;
            }

            currentLine = line;
            narratorUI.UpdateUI();

            AudioController.Instance.PlaySound(line);

            var clipLength = currentLine.AudioClip.length;
            StartCoroutine(ResetAfterLineFinished(clipLength));
            return true;
        }

        private IEnumerator ResetAfterLineFinished(float clipLength)
        {
            var waitTime = Mathf.Max(0, clipLength + audioLengthOffset);
            yield return new WaitForSecondsRealtime(waitTime);
            ResetCurrentLine();
        }

        private void ResetCurrentLine()
        {
            currentLine = null;
            narratorUI.UpdateUI();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(Narrator))]
        private class NarratorEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!(target is Narrator script))
                    return;

                if (!Application.isPlaying)
                    return;

                GUI.enabled = script.testAudio;
                if (GUILayout.Button("Play Test Audio"))
                {
                    script.PlayLine(script.testAudio);
                }

                GUI.enabled = script.testCardData;
                if (GUILayout.Button("Play Test Card Line"))
                {
                    script.PlayLine(script.testCardData);
                }

                GUI.enabled = true;
            }
        }
#endif
    }
}