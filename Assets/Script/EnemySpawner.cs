using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3.5f;
    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private int maxEnemies = 8;

    private Transform player;
    private float timer;

    void Start()
    {
        FindPlayer();

        // Jika prefab belum di-assign, coba load dari Resources/Enemy
        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load<GameObject>("Enemy");
        }

        // Jika masih null, cari musuh yang ada di scene untuk dijadikan template
        if (enemyPrefab == null)
        {
            Enemy existingEnemy = FindAnyObjectByType<Enemy>();
            if (existingEnemy != null)
            {
                enemyPrefab = existingEnemy.gameObject;
            }
        }
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        FindPlayer();
        if (player == null || enemyPrefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        int currentEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (currentEnemies >= maxEnemies) return;

        // Tentukan posisi spawn acak di sekeliling player
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, randomCircle.y, 0f);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.name = "Enemy_AutoSpawned";
        newEnemy.SetActive(true);
    }
}
