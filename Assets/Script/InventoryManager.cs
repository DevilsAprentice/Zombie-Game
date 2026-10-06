using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    InputAction inventoryAction;

    [SerializeField] GameObject InventoryCanvas;

    private bool InventoryActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryCanvas.SetActive(false);
        InventoryActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryAction.triggered && InventoryActive == false )
        {
            Debug.Log("Inventory muncul");
            InventoryActive = true;
            InventoryCanvas.SetActive(true);
            Time.timeScale = 0f; // Pause the game

        }else if (inventoryAction.triggered && InventoryActive == true)
        {
            InventoryActive = false;
            InventoryCanvas.SetActive(false);
            Time.timeScale = 1f; // Resume the game
             Debug.Log("Inventory hilang");
        }
    }
}
