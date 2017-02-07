using UnityEngine;
using System.Collections;

public class MagnusEffect : MonoBehaviour {

    // public float time = 2f;
    // public float mcLimit = 1f;
    public float magnusConstant = 1f;
    float magnusCache = 0f;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        rb.AddForce(Vector3.Cross(rb.angularVelocity, rb.velocity) * magnusConstant);
	}
}
