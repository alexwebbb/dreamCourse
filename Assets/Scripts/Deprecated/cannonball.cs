using UnityEngine;
using System.Collections;

public class cannonball : MonoBehaviour {

    // public Vector3 force;
    public float force;
    Rigidbody rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate() {
        // Debug.Log(rb.velocity.magnitude);
    }

    void OnCollisionEnter(Collision collision) {
        // Debug.Log(rb.angularVelocity + " :enter");
        // rb.AddForce(rb.angularVelocity * force, ForceMode.Impulse);
        // next lets try setting the force based on the angular velocity
    }

    void OnCollisionExit(Collision collision) {
        //Debug.Log(rb.angularVelocity + " :Exit");
        // rb.AddForce(Vector3.back * force, ForceMode.Impulse);
    }
}
