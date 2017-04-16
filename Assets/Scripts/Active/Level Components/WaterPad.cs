using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPad : MonoBehaviour {

    public float power = 5f;
    public float resistance = 2f;

    private void OnTriggerEnter(Collider other) {


        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        // so this KIND OF works right now, need ton add particle effects and clean up the if second case
        if (Vector3.Dot(Vector3.forward, otherRB.velocity.normalized) >0.8f) {
            Debug.Log("bounce");
            if(otherRB.velocity.x > -otherRB.velocity.y || otherRB.velocity.z > -otherRB.velocity.y) otherRB.AddForce(2f * Vector3.up * -otherRB.velocity.y, ForceMode.VelocityChange);
        }

        BounceController bc = other.GetComponent<BounceController>();

        bc.AddDrag = resistance;

        bc.AddConstantForce = new Vector3(0, power, 0);
    }

    private void OnTriggerExit(Collider other) {

        BounceController bc = other.GetComponent<BounceController>();

        bc.ResetDrag();

        bc.ResetConstantForce();

    }
}
