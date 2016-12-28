using UnityEngine;
using System.Collections;

public class SpinScript : MonoBehaviour {

    // public float cutoff;
    public float power;
    public int bounceLimit;
    int bounceCount;
    float spinFloat;
    bool collisionActive;
    Rigidbody rb;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!collisionActive) {
            spinFloat = Vector3.Cross(rb.angularVelocity, rb.velocity).z;
        }
    }

    void OnCollisionEnter(Collision collision) {
        collisionActive = true;

        if (bounceCount > 2 && bounceCount < bounceLimit) {
            if (spinFloat < 0) {
                rb.AddForce(Vector3.forward * power, ForceMode.Impulse);
            } else {
                rb.AddForce(Vector3.back * power, ForceMode.Impulse);
            }
        }
        bounceCount++;
    }

    void OnCollisionExit(Collision collision) {
        
        

        collisionActive = false;
    }
}
