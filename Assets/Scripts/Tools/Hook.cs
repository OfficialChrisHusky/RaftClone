using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
    
    [SerializeField] private bool thrown = false;
    [SerializeField] private float throwForce = 20.0f;
    [SerializeField] private bool canRetract = false;
    [SerializeField] private float retractSpeed = 1.0f;

    [SerializeField] private List<Catchable> catched = new List<Catchable>();

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

        foreach (Catchable catchable in catched) {

            catchable.transform.position = transform.position;

        }

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

        Vector3 moveForce = (destination - transform.position).normalized * retractSpeed;
        transform.position = Vector3.MoveTowards(transform.position, destination, retractSpeed * Time.deltaTime);

    }

    private void FinishRetract() {

        thrown = false;
        canRetract = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.parent = parent;
        transform.localPosition = new Vector3(0.0f, 0.4f, 1.6f);

        foreach (Catchable catchable in catched) {

            Inventory.instance.AddItem(catchable.Item, catchable.Amount);
            CatchableManager.instance.RemoveCatchable(catchable);

        }
        catched.Clear();

    }

    void OnTriggerEnter(Collider other) {

        if (!thrown) return;
        
        if(other.tag == "Catchable") {
            
            catched.Add(other.GetComponent<Catchable>());

            other.transform.position = transform.position;
            other.enabled = false;

        } else if(other.tag == "Water") canRetract = true;

    }
    void OnCollisionEnter(Collision other) {
        
        if(other.gameObject.tag == "Raft" || other.gameObject.tag == "Player") FinishRetract();

    }

}