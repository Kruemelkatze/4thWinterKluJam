using UnityEngine;

namespace Cards
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private Card card;
        [SerializeField] private bool randomColor = false;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void UpdateFields()
        {
        }

        public void Init()
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.sortingOrder += card.cardNumber;
                if (randomColor)
                {
                    spriteRenderer.color = new Color(Random.value, Random.value, Random.value, 1);
                }
            }
        }
    }
}