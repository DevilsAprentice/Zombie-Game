using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateSlots();
        InventoryCanvas.SetActive(false);
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == false)
        {
            InventoryActive = true;
            Time.timeScale = 0f;
            Debug.Log("Inventory is active");
            InventoryCanvas.SetActive(true);
        } else if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == true)
        {
            InventoryActive = false;
            Time.timeScale = 1f;
            Debug.Log("Inventory is deactivated");
            InventoryCanvas.SetActive(false);
        }
    }

    // Build the slot objects at runtime so no manual wiring is needed in the scene
    private void CreateSlots()
    {
        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");
        if (slotPrefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found in a Resources folder.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        // Put the slots inside the GridLayoutGroup container (InventorySlots), if there is one
        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
        }
    }

   public void AddItem(ItemDetails item)
{
    Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

    if (itemSlot == null || itemSlot.Length == 0)
    {
        Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was lost.");
        return;
    }

    int remaining = item.Quantity;

    // 1) First, fill existing stacks of the same item
    for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
    {
        if (itemSlot[i].CanStackWith(item.ItemName))
        {
            remaining = itemSlot[i].AddQuantity(remaining);
        }
    }

    // 2) Then use empty slots for whatever is left
    for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
    {
        if (itemSlot[i].IsEmpty)
        {
            remaining = itemSlot[i].AddItem(item, remaining);
        }
    }

    if (remaining > 0)
    {
        Debug.LogWarning("Inventory is full, " + remaining + " x '" + item.ItemName + "' was lost.");
    }
}
public void DeselectAllSlots()
{
    foreach (ItemSlot slot in itemSlot)
    {
        slot.isItemSelected = false;
        slot.selectedPanel.SetActive(false);
    }
}
    
}