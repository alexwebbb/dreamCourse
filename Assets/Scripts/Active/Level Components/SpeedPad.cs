using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPad : MonoBehaviour {

    public float power = 40f;

    private void OnCollisionEnter(Collision collision) {

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * power, ForceMode.VelocityChange);
    }
}
