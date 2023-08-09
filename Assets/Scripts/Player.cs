using UnityEngine;

public class Player : MonoBehaviour {
    
    public static Player instance;
    void Awake() { instance = this; }

    public bool gameStarted = false;
    public Transform head;


    void Update() {
        
        if (!gameStarted && Input.GetKeyDown(KeyCode.Mouse0)) { gameStarted = true; return; }
        if (!gameStarted) return;

    }

}