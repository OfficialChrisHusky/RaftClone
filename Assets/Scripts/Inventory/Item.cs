using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    
    [SerializeField] private string name = "Item";
    [SerializeField] private uint id = 0;
    [SerializeField] private uint stackLimit = 10;

    [Header("Visual")]
    [SerializeField] private Mesh mesh;
    [SerializeField] private List<Material> materials = new List<Material>();
    public Color color = Color.white;

    public string Name { get { return name; } }
    public uint ID { get { return id; } }
    public uint StackLimit { get { return stackLimit;} }
    public Mesh Mesh { get { return mesh; } }
    public List<Material> Materials { get { return materials; } }

}