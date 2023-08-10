using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public static Inventory instance;
    void Awake() { instance = this;}

    [SerializeField] private bool open = false;
    [SerializeField] private Dictionary<uint, uint> items = new Dictionary<uint, uint>();

    [Header("Static")]
    [SerializeField] private List<Item> allItems = new List<Item>();
    [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

    [Header("UI")]
    [SerializeField] private GameObject inventoryUI;

    public void AddItem(Item item, uint amount = 1) { AddItem(item.ID, amount); }
    public void AddItem(uint itemID, uint amount = 1) {

        Debug.Log("Item: " + itemID + ", Amount: " + amount);
        ItemSlot slot = null;
        foreach(ItemSlot tmp in slots) {

            if(tmp.Item != null && tmp.Item.ID != itemID) continue;
            slot = tmp;
            break;

        }
        if(slot == null) return;

        if (items.ContainsKey(itemID)) { items[itemID] += amount; }
        else { items.Add(itemID, amount); }

        if (slot.Item != null && slot.Item.ID != itemID) return;
        slot.AddItem(allItems[(int) itemID], amount);

    }

    public void RemoveItem(Item item, uint amount = 1) { RemoveItem(item.ID, amount); }
    public void RemoveItem(uint itemID, uint amount = 1) {

        if (!HasItem(itemID, amount)) return;

        items[itemID] -= amount;

    }

    public bool HasItem(Item item, uint amount = 1) { return HasItem(item.ID, amount); }
    public bool HasItem(uint itemID, uint amount = 1) {
        
        if(!items.ContainsKey(itemID)) return false;
        if(items[itemID] < amount) return false;

        return true;

    }

    private void OpenInventory() {

        open = !open;

        inventoryUI.SetActive(open);
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;

        Player.instance.gamePaused = open;

    }

    void Update() {
        
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        OpenInventory();

    }
    
}