using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TMP_Text healthText;

    private PlayerHealth playerHealth;

    void Awake()
    {
        if (healthSlider == null) healthSlider = GetComponent<Slider>();
        if (healthSlider == null) healthSlider = GetComponentInChildren<Slider>(true);
    }

    void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.CurrentHp, playerHealth.MaxHp);
        }
        else
        {
            Debug.LogWarning("PlayerHealthBar: PlayerHealth tidak ditemukan di scene!", this);
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int currentHp, int maxHp)
    {
        float ratio = maxHp > 0 ? (float)currentHp / maxHp : 0f;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHp;
        }

        if (healthFillImage != null)
        {
            healthFillImage.color = Color.green;
            healthFillImage.fillAmount = ratio;
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {currentHp} / {maxHp}";
        }
    }

    // Health bar hijau polos di pojok kiri atas
    void OnGUI()
    {
        if (playerHealth == null) return;

        // Dimensi Bar Polos
        float barWidth = 200f;
        float barHeight = 22f;
        float x = 20f;
        float y = 20f;

        float hpPercent = Mathf.Clamp01((float)playerHealth.CurrentHp / playerHealth.MaxHp);

        // 1. Border hitam tipis polos di luar bar
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(x - 2, y - 2, barWidth + 4, barHeight + 4), Texture2D.whiteTexture);

        // 2. Latar bar kosong (abu-abu gelap polos)
        GUI.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        GUI.DrawTexture(new Rect(x, y, barWidth, barHeight), Texture2D.whiteTexture);

        // 3. Health Bar Hijau Polos
        GUI.color = new Color(0.18f, 0.8f, 0.25f, 1f); // Hijau polos
        GUI.DrawTexture(new Rect(x, y, barWidth * hpPercent, barHeight), Texture2D.whiteTexture);

        // 4. Teks HP putih sederhana
        GUI.color = Color.white;
        GUI.Label(new Rect(x + 6, y + 2, barWidth, barHeight), $"HP: {playerHealth.CurrentHp} / {playerHealth.MaxHp}");
    }
}
