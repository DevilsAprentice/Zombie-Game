using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private TMP_Text itemDescriptionText;

    void Awake()
    {
        SetupUIElementsIfMissing();
        ClearDetails();
    }

    // Otomatis membuat elemen UI jika panel masih polos/kosong
    private void SetupUIElementsIfMissing()
    {
        // 1. Icon Image
        if (itemIcon == null)
        {
            Transform iconTr = transform.Find("DetailIcon");
            if (iconTr != null)
            {
                itemIcon = iconTr.GetComponent<Image>();
            }
            else
            {
                GameObject iconObj = new GameObject("DetailIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                iconObj.transform.SetParent(transform, false);
                RectTransform rt = iconObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100, 100);
                itemIcon = iconObj.GetComponent<Image>();
                itemIcon.preserveAspect = true;
            }
        }

        // 2. Name Text
        if (itemNameText == null)
        {
            Transform nameTr = transform.Find("DetailName");
            if (nameTr != null)
            {
                itemNameText = nameTr.GetComponent<TMP_Text>();
            }
            else
            {
                GameObject nameObj = new GameObject("DetailName", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                nameObj.transform.SetParent(transform, false);
                itemNameText = nameObj.GetComponent<TextMeshProUGUI>();
                itemNameText.fontSize = 24;
                itemNameText.fontStyle = FontStyles.Bold;
                itemNameText.alignment = TextAlignmentOptions.Center;
                itemNameText.color = Color.white;
            }
        }

        // 3. Quantity Text
        if (itemQuantityText == null)
        {
            Transform qtyTr = transform.Find("DetailQuantity");
            if (qtyTr != null)
            {
                itemQuantityText = qtyTr.GetComponent<TMP_Text>();
            }
            else
            {
                GameObject qtyObj = new GameObject("DetailQuantity", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                qtyObj.transform.SetParent(transform, false);
                itemQuantityText = qtyObj.GetComponent<TextMeshProUGUI>();
                itemQuantityText.fontSize = 18;
                itemQuantityText.alignment = TextAlignmentOptions.Center;
                itemQuantityText.color = Color.yellow;
            }
        }

        // 4. Description Text
        if (itemDescriptionText == null)
        {
            Transform descTr = transform.Find("DetailDescription");
            if (descTr != null)
            {
                itemDescriptionText = descTr.GetComponent<TMP_Text>();
            }
            else
            {
                GameObject descObj = new GameObject("DetailDescription", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                descObj.transform.SetParent(transform, false);
                itemDescriptionText = descObj.GetComponent<TextMeshProUGUI>();
                itemDescriptionText.fontSize = 16;
                itemDescriptionText.alignment = TextAlignmentOptions.Center;
                itemDescriptionText.color = new Color(0.9f, 0.9f, 0.9f);
            }
        }
    }

    public void ShowDetails(ItemDetails details, int quantity)
    {
        if (details == null)
        {
            ClearDetails();
            return;
        }

        if (itemIcon != null)
        {
            itemIcon.sprite = details.ItemIcon;
            itemIcon.enabled = details.ItemIcon != null;
        }

        if (itemNameText != null)
        {
            itemNameText.text = details.ItemName;
            itemNameText.enabled = true;
        }

        if (itemQuantityText != null)
        {
            itemQuantityText.text = $"Jumlah: {quantity}";
            itemQuantityText.enabled = true;
        }

        if (itemDescriptionText != null)
        {
            string desc = !string.IsNullOrEmpty(details.ItemDescription) ? details.ItemDescription : "Tidak ada deskripsi.";
            itemDescriptionText.text = desc;
            itemDescriptionText.enabled = true;
        }
    }

    public void ClearDetails()
    {
        if (itemIcon != null) itemIcon.enabled = false;
        if (itemNameText != null) itemNameText.text = "Pilih item untuk melihat detail";
        if (itemQuantityText != null) itemQuantityText.text = "";
        if (itemDescriptionText != null) itemDescriptionText.text = "";
    }
}
