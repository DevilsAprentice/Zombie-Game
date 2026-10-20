using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Details")]
public class ItemDetails : ScriptableObject
{
    public string ItemName;
    public int Quantity = 1;
    public Sprite ItemIcon;
}