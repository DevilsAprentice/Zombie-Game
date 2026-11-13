using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalKoin;
    private int koinTerkumpul = 0;

    void Awake()
    {
        // Pastikan EnemySpawner aktif di GameManager
        if (GetComponent<EnemySpawner>() == null)
        {
            gameObject.AddComponent<EnemySpawner>();
        }
    }

    void Start()
    {
        totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;

        // Spawn beberapa Potion (Collectible Item Tipe 2) di sekitar arena
        SpawnInitialPotions();
    }

    private void SpawnInitialPotions()
    {
        GameObject potionPrefab = Resources.Load<GameObject>("Potion");
        if (potionPrefab == null) return;

        // Periksa apakah sudah ada Potion di scene
        Item[] allItems = FindObjectsByType<Item>(FindObjectsSortMode.None);
        bool hasPotion = false;
        foreach (Item it in allItems)
        {
            if (it.name.Contains("Potion"))
            {
                hasPotion = true;
                break;
            }
        }

        if (!hasPotion)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 center = player != null ? player.transform.position : Vector3.zero;

            Vector3[] spawnOffsets = new Vector3[]
            {
                new Vector3(2.5f, 1.5f, 0f),
                new Vector3(-3.0f, -2.0f, 0f),
                new Vector3(3.5f, -3.0f, 0f)
            };

            for (int i = 0; i < spawnOffsets.Length; i++)
            {
                GameObject pot = Instantiate(potionPrefab, center + spawnOffsets[i], Quaternion.identity);
                pot.name = $"Potion_Collectible_{i + 1}";
            }

            Debug.Log("Spawned 3 initial Potions (Collectible Item Tipe 2) in arena.");
        }
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
        Debug.Log("KAMU MENANG! Semua koin terkumpul!");
    }
}
