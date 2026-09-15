    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public int totalKoin;
        private int koinTerkumpul = 0;

        void Start()
        {
            
            totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;
        }

        public void AmbilKoin()
        {
            koinTerkumpul++;

        
            if (koinTerkumpul == totalKoin)
            {
                Menang();
            }
        }

        void Menang()
        {
            Debug.Log("KAMU MENANG!");
        }
    }