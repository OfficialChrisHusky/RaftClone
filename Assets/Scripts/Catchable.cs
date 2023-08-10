using UnityEngine;

public class Catchable : MonoBehaviour {
    
    [SerializeField] private Item item;
    [SerializeField] private uint amount = 1;

    public void Setup(Item item, uint amount) {

        this.item = item;
        this.amount = amount;

    }

    void Update() {
        
        transform.position += transform.parent.forward * CatchableManager.instance.WaveStrength * Time.deltaTime;

    }

    public Item Item { get { return item;} }
    public uint Amount { get { return amount;} }

}