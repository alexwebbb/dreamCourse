using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPad : MonoBehaviour {

    public float power = 40f;

    private void OnCollisionEnter(Collision collision) {

        collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.VelocityChange);
    }
}
