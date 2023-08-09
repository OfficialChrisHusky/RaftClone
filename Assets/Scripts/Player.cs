using UnityEngine;

public class Player : MonoBehaviour {
    
    public static Player instance;
    void Awake() { instance = this; }

    public bool gameStarted = false;

    [SerializeField] private Hook hook;

    void Update() {
        
        if (!gameStarted && Input.GetKeyDown(KeyCode.Mouse0)) gameStarted = true;
        if (!Player.instance.gameStarted) return;

    }

}