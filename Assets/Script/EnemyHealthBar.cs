using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        // Otomatis mengambil komponen Slider di objek ini jika belum diisi di Inspector
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }
    }

    private void OnEnable()
    {
        if (enemy != null)
            enemy.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (enemy != null)
            enemy.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int currentHp, int maxHp)
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHp / maxHp;
        }
    }
}