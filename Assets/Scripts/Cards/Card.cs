using Extensions;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;

        [ReadOnly] public int cardNumber;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Init(int cn)
        {
            cardNumber = cn;
            cardDisplay.Init();
        }
    }
}