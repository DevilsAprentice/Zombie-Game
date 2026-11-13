using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private Transform slotPanel;
    [SerializeField] private ItemDescriptionUI itemDescriptionUI;
    [SerializeField] private int slotCount = 16;
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    void Awake()
    {
        CreateSlots();
        InitDescriptionUI();

        if (InventoryCanvas != null)
        {
            InventoryCanvas.SetActive(false);
        }
    }

    void Start()
    {
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryActive = false;
    }

    private void InitDescriptionUI()
    {
        if (InventoryCanvas == null) return;

        if (itemDescriptionUI == null)
        {
            itemDescriptionUI = InventoryCanvas.GetComponentInChildren<ItemDescriptionUI>(true);
        }

        // Jika belum ada komponen ItemDescriptionUI di panel ItemDescription, pasang otomatis
        if (itemDescriptionUI == null)
        {
            Transform[] allChildren = InventoryCanvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allChildren)
            {
                if (t.name == "ItemDescription")
                {
                    itemDescriptionUI = t.gameObject.AddComponent<ItemDescriptionUI>();
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryAction != null && inventoryAction.triggered && InventoryCanvas != null)
        {
            if (!InventoryActive)
            {
                InventoryActive = true;
                Time.timeScale = 0f;
                Debug.Log("Inventory is active");
                InventoryCanvas.SetActive(true);
                DeselectAllSlots();
                KosongkanDetail();
            }
            else
            {
                InventoryActive = false;
                Time.timeScale = 1f;
                Debug.Log("Inventory is deactivated");
                InventoryCanvas.SetActive(false);
            }
        }
    }

    private void CreateSlots()
    {
        if (InventoryCanvas == null)
        {
            Debug.LogError("InventoryCanvas is not assigned in the Inspector!", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");
        if (slotPrefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found in Assets/Resources/ folder.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        // Determine the parent transform for the slots
        Transform parent;
        if (slotPanel != null)
        {
            parent = slotPanel;
        }
        else
        {
            // Priority 1: Find GridLayoutGroup (typically inside the slot container like 'InventorySlot')
            GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
            if (grid != null)
            {
                parent = grid.transform;
            }
            else
            {
                // Priority 2: Any other LayoutGroup
                LayoutGroup layout = InventoryCanvas.GetComponentInChildren<LayoutGroup>(true);
                if (layout != null)
                {
                    parent = layout.transform;
                }
                else
                {
                    parent = InventoryCanvas.transform;
                    Debug.LogWarning("No slotPanel or LayoutGroup found. Slots placed directly on Canvas.", this);
                }
            }
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
            itemSlot[i].gameObject.name = "ItemSlot_" + i;
            itemSlot[i].gameObject.SetActive(true);
        }

        Debug.Log("Created " + slotCount + " slots under parent: " + parent.gameObject.name);
    }

    public void AddItem(ItemDetails item)
    {
        if (item == null) return;

        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        if (itemSlot == null || itemSlot.Length == 0)
        {
            CreateSlots();
        }

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was lost.");
            return;
        }

        int remaining = item.Quantity;

        // 1) First, fill existing stacks of the same item
        for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
        {
            if (itemSlot[i] != null && itemSlot[i].CanStackWith(item.ItemName))
            {
                remaining = itemSlot[i].AddQuantity(remaining);
            }
        }

        // 2) Then use empty slots for whatever is left
        for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
        {
            if (itemSlot[i] != null && itemSlot[i].IsEmpty)
            {
                remaining = itemSlot[i].AddItem(item, remaining);
            }
        }

        if (remaining > 0)
        {
            Debug.LogWarning("Inventory is full, " + remaining + " x '" + item.ItemName + "' was lost.");
        }
    }

    public void TampilkanDetail(ItemDetails details, int quantity)
    {
        if (itemDescriptionUI == null) InitDescriptionUI();
        if (itemDescriptionUI != null)
        {
            itemDescriptionUI.ShowDetails(details, quantity);
        }
    }

    public void KosongkanDetail()
    {
        if (itemDescriptionUI == null) InitDescriptionUI();
        if (itemDescriptionUI != null)
        {
            itemDescriptionUI.ClearDetails();
        }
    }

    public void DeselectAllSlots()
    {
        if (itemSlot == null) return;
        foreach (ItemSlot slot in itemSlot)
        {
            if (slot != null)
            {
                slot.isItemSelected = false;
                if (slot.selectedPanel != null)
                {
                    slot.selectedPanel.SetActive(false);
                }
            }
        }
    }
}
