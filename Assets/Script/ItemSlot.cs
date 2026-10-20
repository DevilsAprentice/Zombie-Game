using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFull;

    // Item Slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int maxStackSize = 99;

    // Selection
    [SerializeField] public GameObject selectedPanel;
    public bool isItemSelected;
    private InventoryManager inventoryManager;

    public bool IsEmpty => itemQuantity == 0;

    private void Awake()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();

        // Hide everything until needed
        itemImage.enabled = false;
        quantityText.enabled = false;
        if (selectedPanel != null) selectedPanel.SetActive(false);
    }

    public bool CanStackWith(string otherItemName)
    {
        return !IsEmpty && !isFull && itemName == otherItemName;
    }

    public int AddItem(ItemDetails item, int amount)
    {
        itemName = item.ItemName;
        itemIcon = item.ItemIcon;
        return AddQuantity(amount);
    }

    public int AddQuantity(int amount)
    {
        int space = maxStackSize - itemQuantity;
        int toAdd = Mathf.Min(amount, space);

        itemQuantity += toAdd;
        isFull = itemQuantity >= maxStackSize;

        quantityText.text = itemQuantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemIcon;
        itemImage.enabled = true;

        return amount - toAdd;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.DeselectAllSlots();
            selectedPanel.SetActive(true);
            isItemSelected = true;
        }
    }
}