using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHp = 100;
    private int currentHp;

    public event Action<int, int> OnHealthChanged;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    void Awake()
    {
        currentHp = maxHp;
    }

    void Start()
    {
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void KenaDamage(int jumlah)
    {
        if (currentHp <= 0) return;

        currentHp -= jumlah;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log($"Player menerima damage {jumlah}! Sisa HP: {currentHp}/{maxHp}");
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

        Debug.Log($"Player pulih {jumlah} HP! Sisa HP: {currentHp}/{maxHp}");
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    private void Mati()
    {
        Debug.Log("Player telah mati! Game Over.");
        // Nonaktifkan gerakan saat mati
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }
}
