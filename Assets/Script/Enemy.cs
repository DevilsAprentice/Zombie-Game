using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Status Health")]
    [SerializeField] private int maxHp = 100;
    private int currentHp;

    [Header("Movement & Attack")]
    public float ms = 2f;
    [SerializeField] private int attackDamage = 20;

    // Event opsional untuk update UI Health Bar (mengirim currentHp & maxHp)
    public event Action<int, int> OnHealthChanged;

    protected Transform player;

    [Header("Pengaturan State Machine")]
    [SerializeField] private float jarakDeteksi = 8f;   // masuk CHASE
    [SerializeField] private float jarakSerang = 1.2f;  // masuk ATTACK
    [SerializeField] private float jedaSerang = 0.5f;   // jeda sebelum serang

    private StateZombie state = StateZombie.idle;
    private float waktuSerangTerakhir;

    protected virtual void Start()
    {
        currentHp = maxHp;
        OnHealthChanged?.Invoke(currentHp, maxHp);

        FindPlayer();
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        FindPlayer();
        PeriksaTransisi();

        switch (state)
        {
            case StateZombie.idle:
                PerilakuIdle();
                break;

            case StateZombie.patrol:
                PerilakuPatrol();
                break;

            case StateZombie.chase:
                PerilakuChase();
                break;

            case StateZombie.attack:
                PerilakuAttack();
                break;
        }
    }   

    // HEALTH SYSTEM & DAMAGE
    public void KenaDamage(int jumlah)
    {
        if (currentHp <= 0) return;

        currentHp -= jumlah;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {currentHp}");
        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Mati();
        }
    }

    public void Heal(int jumlah)
    {
        if (currentHp <= 0) return;

        currentHp += jumlah;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log($"{gameObject.name} di-heal {jumlah}, HP sekarang: {currentHp}");
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public float GetHealthPercentage()
    {
        return (float)currentHp / maxHp;
    }

    protected virtual void Mati()
    {
        Debug.Log($"{gameObject.name} hancur!");
        Destroy(gameObject);
    }

    // STATE TRANSITION
    public float JarakKePlayer()
    {
        if (player == null)
            return Mathf.Infinity;

        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= jarakSerang)
        {
            state = StateZombie.attack;
        }
        else if (jarak <= jarakDeteksi)
        {
            state = StateZombie.chase;
        }
        else
        {
            // Jika player terlihat atau dalam arena, musuh tetap mengejar player
            state = StateZombie.chase;
        }
    }

    void PerilakuIdle()
    {
        Kejar();
    }

    void PerilakuPatrol()
    {
        Kejar();
    }

    void PerilakuChase()
    {
        Kejar();
    }

    void PerilakuAttack()
    {
        if (Time.time >= waktuSerangTerakhir + jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }

    // MOVEMENT & ATTACK
    public void Kejar()
    {
        if (player == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        if (player != null)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.KenaDamage(attackDamage);
                Debug.Log($"{gameObject.name} menyerang Player dan memberikan damage {attackDamage}!");
            }
        }

        // Ketentuan: Ketika sudah menyerang, musuh akan hancur
        Mati();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Serang();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Serang();
        }
    }
}
