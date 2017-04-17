using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPad : MonoBehaviour {

    public float power = 5f;
    public float resistance = 2f;
    public float skipThreshhold = 0.95f;
    public float skipPower = 1.2f;

    private void OnTriggerEnter(Collider other) {


        // I think this pretty much works now, need to add particle effects
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        // Debug.Log(Mathf.Abs(Vector3.Dot(new Vector3(otherRB.velocity.x, 0, otherRB.velocity.z).normalized, otherRB.velocity.normalized)));
        // check the dot product of the velocity vs the horizontal plane
        if (Mathf.Abs(Vector3.Dot(new Vector3(otherRB.velocity.x, 0, otherRB.velocity.z).normalized, otherRB.velocity.normalized)) > skipThreshhold) {
            // compare the magnitude of horizontal movement vs vertical, if horizontal is greater, bounce can occur
            if (Vector3.SqrMagnitude(new Vector3(otherRB.velocity.x, 0, otherRB.velocity.z))/2 > Vector3.SqrMagnitude(new Vector3(0, otherRB.velocity.y, 0))) {
                // adjust velocity, slowing horizontal movement, adding vertical velocity
                otherRB.velocity = new Vector3(otherRB.velocity.x / 2, skipPower * -otherRB.velocity.y, otherRB.velocity.z / 2);
            }
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
