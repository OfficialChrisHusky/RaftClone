using UnityEngine;

public class Hook : MonoBehaviour {
    
    [SerializeField] private bool thrown = false;
    [SerializeField] private float throwForce = 20.0f;
    [SerializeField] private bool canRetract = false;
    [SerializeField] private float retractSpeed = 1.0f;

    private Rigidbody rb;

    private Vector3 prevPosition;
    private Transform parent;

    void Start() {
        
        rb = GetComponent<Rigidbody>();
        parent = transform.parent;

    }
    void Update() {
        
        if (!Player.instance.gameStarted || Player.instance.gamePaused) return;
        if (!thrown) {

            transform.localPosition = new Vector3(0.0f, 0.4f, 1.5f);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1.0f, 0.2f, 2.0f);

            if(Input.GetKeyDown(KeyCode.Mouse0)) Throw();
            return;
            
        }
        if (Input.GetKey(KeyCode.Mouse0)) Retract(new Vector3(Player.instance.transform.position.x, transform.position.y, Player.instance.transform.position.z));

    }

    private void Throw() {

        thrown = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.parent = null;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

    }
    private void Retract(Vector3 destination) {

        if (!thrown) return;
        if (!canRetract) return;

        rb.AddForce((destination - transform.position).normalized * retractSpeed);

    }

    private void FinishRetract() {

        thrown = false;
        canRetract = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.parent = parent;
        transform.localPosition = new Vector3(0.0f, 0.4f, 1.6f);

    }

    void OnTriggerEnter(Collider other) {

        if (!thrown) return;
        
        if(other.tag == "Catchable") Debug.Log("Catched Something!");
        else if(other.tag == "Water") canRetract = true;

    }
    void OnCollisionEnter(Collision other) {
        
        if(other.gameObject.tag == "Raft" || other.gameObject.tag == "Player") FinishRetract();

    }

    public bool Thrown() { return thrown; }

}