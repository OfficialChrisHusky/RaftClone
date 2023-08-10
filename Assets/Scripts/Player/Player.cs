using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public static Player instance;
    void Awake() { instance = this; }

    public bool gameStarted = false;
    public bool gamePaused = false;
    public Transform head;

    [SerializeField] private int minItemsWhenAdd = 1;
    [SerializeField] private int maxItemsWhenAdd = 10;

    void Update() {
        
        if (!gameStarted && Input.GetKeyDown(KeyCode.Mouse0)) { gameStarted = true; return; }
        if (!gameStarted) return;

        if(Input.GetKeyDown(KeyCode.X)) {

            Inventory.instance.AddItem((uint) Random.Range(0, 2), (uint) Random.Range(minItemsWhenAdd, maxItemsWhenAdd + 1));

        }
        if(Input.GetKeyDown(KeyCode.Y)) {

            Inventory.instance.RemoveItem((uint) Random.Range(0, 2), (uint) Random.Range(minItemsWhenAdd, maxItemsWhenAdd + 1));

        }

    }

}