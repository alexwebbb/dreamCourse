using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedPad : MonoBehaviour {

    public float power = 15f;

    private void OnCollisionEnter(Collision collision) {

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        Debug.Log(transform.forward);
        rb.AddForce(transform.forward * power, ForceMode.VelocityChange);
    }
}
