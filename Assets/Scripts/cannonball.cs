using UnityEngine;
using System.Collections;

public class cannonball : MonoBehaviour {

    public Vector3 force;
    Rigidbody rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate() {
        // Debug.Log(rb.velocity.magnitude);
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision");
        rb.AddForce(force);
    }
}
