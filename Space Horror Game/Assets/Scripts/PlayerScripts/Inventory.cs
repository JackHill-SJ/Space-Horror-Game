using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    List<GameObject> InventoryItems = new List<GameObject>();
    int generatorBatteries;

    public void AddItemToInventory(GameObject Item)
    {
        if (!InventoryItems.Contains(Item))
        {
            //InventoryItems.Add(Item);
            //InventoryItems[0] = Item;
            InventoryItems.Insert(0, Item);
            Item.transform.SetParent(this.transform);
            //Item.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            Item.SetActive(false);
        }
    }

    public void PlaceItemFromInventory(GameObject Portal)
    {
        if (Portal != null)
        {
            InventoryItems[0].transform.SetParent(Portal.transform);
            InventoryItems[0].SetActive(true);
            InventoryItems.RemoveAt(0);
        }
    }
}
