using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    
    [SerializeField] private string name = "Item";
    [SerializeField] private uint id = 0;
    [SerializeField] private uint stackLimit = 10;
    public Color color = Color.white;

    public string Name { get { return name; } }
    public uint ID { get { return id; } }
    public uint StackLimit { get { return stackLimit;} }

}