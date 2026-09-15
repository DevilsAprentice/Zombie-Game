using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Status Health")]
    [SerializeField] private int maxHp = 100;
    private int currentHp;

    public float ms = 2f;

    // Event opsional untuk update UI Health Bar (mengirim currentHp & maxHp)
    public event Action<int, int> OnHealthChanged;

    protected Transform player;

    [Header("Pengaturan State Machine")]
    [SerializeField] private float jarakDeteksi = 6f;   // masuk CHASE
    [SerializeField] private float jarakSerang = 1.2f;  // masuk ATTACK
    [SerializeField] private float jedaSerang = 1f;     // jeda antar serangan

    private StateZombie state = StateZombie.idle;
    private float waktuSerangTerakhir;

    protected virtual void Start()
    {
        // Set HP saat game mulai
        currentHp = maxHp;
        OnHealthChanged?.Invoke(currentHp, maxHp);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
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
        if (currentHp <= 0) return; // Cegah damage jika sudah mati

        currentHp -= jumlah;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Batasi HP minimal 0

        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {currentHp}");

        OnHealthChanged?.Invoke(currentHp, maxHp); // Kabari UI/Health Bar

        if (currentHp <= 0)
        {
            Mati();
        }
    }

    public void Heal(int jumlah)
    {
        if (currentHp <= 0) return; // Zombie mati tidak bisa di-heal

        currentHp += jumlah;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Batasi HP tidak melebihi maxHp

        Debug.Log($"{gameObject.name} di-heal {jumlah}, HP sekarang: {currentHp}");

        OnHealthChanged?.Invoke(currentHp, maxHp); // Kabari UI/Health Bar
    }

    public float GetHealthPercentage()
    {
        return (float)currentHp / maxHp;
    }

    protected virtual void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
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
            state = StateZombie.patrol;
        }
    }

    // STATE BEHAVIOUR


    void PerilakuIdle()
    {
        Debug.Log(name + ": IDLE");
    }

    void PerilakuPatrol()
    {
        Debug.Log(name + ": PATROL");
        // Tambahkan logika patrol di sini
    }

    void PerilakuChase()
    {
        Debug.Log(name + ": CHASE");
        Kejar();
    }

    void PerilakuAttack()
    {
        Debug.Log(name + ": ATTACK");

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
        Debug.Log("Enemy menyerang!");
    }
}