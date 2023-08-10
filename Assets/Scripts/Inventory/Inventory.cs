using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public static Inventory instance;
    void Awake() { instance = this; }

    [SerializeField] private bool open = false;
    [SerializeField] private Dictionary<uint, uint> items = new Dictionary<uint, uint>();

    [Header("Static")]
    [SerializeField] private List<Item> allItems = new List<Item>();
    [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

    [Header("UI")]
    [SerializeField] private GameObject inventoryUI;

    private List<int> emptySlots = new List<int>();

    void Start() {
        
        for(int i = 0; i < slots.Count; i++) { emptySlots.Add(i); }

    }

    public void AddItem(Item item, uint amount = 1) { AddItem(item.ID, amount); }
    public void AddItem(uint itemID, uint amount = 1) {

        Debug.Log(itemID + ": " + amount);

        uint amountToAdd = amount;

        // Try to Fill slots that already have the requested item
        foreach (ItemSlot slot in slots) {

            if (slot.Item == null || slot.Item.ID != itemID) continue;
            uint freeSpace = slot.Item.StackLimit - slot.Amount;

            // If we can fit it into this slot, do and finish
            if (freeSpace >= amountToAdd) {

                slot.AddItem(slot.Item, amountToAdd);
                amountToAdd = 0;
                break;

            }

            // If we cant, fill it to the max, and try to fit the remainder into the next slot
            slot.AddItem(slot.Item, freeSpace);
            amountToAdd -= freeSpace;

        }

        // If we didnt fit all of it in the previous step
        List<int> tmp = emptySlots;
        for (int i = 0; amountToAdd > 0 && i < tmp.Count; i++) {
            
            int slotIndex = tmp[i];
            ItemSlot slot = slots[slotIndex];

            // If we can fit the items into this single slot
            if (amountToAdd <= allItems[(int) itemID].StackLimit) {

                slot.AddItem(allItems[(int) itemID], amountToAdd);
                emptySlots.RemoveAt(i);
                amountToAdd = 0;
                break;

            }

            // If we cant, fill up the entire slot and continue onto the next
            slot.AddItem(allItems[(int) itemID], allItems[(int) itemID].StackLimit);
            amountToAdd -= allItems[(int) itemID].StackLimit;
            emptySlots.RemoveAt(i);

        }

        // If we werent able to fit all of the items into the inventory
        if (amountToAdd > 0) {

            if (items.ContainsKey(itemID)) { items[itemID] += amount - amountToAdd; }
            else { items.Add(itemID, amount - amountToAdd); }
            return;

        }

        // We were successfully able to add all of the items into the inventory
        if (items.ContainsKey(itemID)) { items[itemID] += amount; }
        else { items.Add(itemID, amount); }

    }

    public void RemoveItem(Item item, uint amount = 1) { RemoveItem(item.ID, amount); }
    public void RemoveItem(uint itemID, uint amount = 1) {

        if (!HasItem(itemID, amount)) return;

        uint amountToRemove = amount;
        for (int i = 0; i < slots.Count; i++) {
            
            ItemSlot slot = slots[i];
            if (slot.Item == null || slot.Item.ID != itemID) continue;

            // If this slot has enough to remove what we need, remove it
            if (slot.Amount > amountToRemove) {

                slot.RemoveItem(slot.Item, amountToRemove);
                amountToRemove = 0;
                break;

            }

            // If it doesnt, empty it and continue to the next one
            amountToRemove -= slot.Amount;
            slot.RemoveItem(slot.Item, slot.Amount);
            emptySlots.Add(i);

        }
        emptySlots.Sort();

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

    public Item ItemFromID(uint itemID) { return allItems[(int) itemID]; }
    
}