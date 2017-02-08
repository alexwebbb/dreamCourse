using UnityEngine;
using System.Collections;

public class MagnusEffect : MonoBehaviour {

    public float magnusConstant = 1f;
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
