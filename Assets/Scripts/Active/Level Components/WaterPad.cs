using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPad : MonoBehaviour {

    public float power = 5f;
    public float resistance = 2f;

    private void OnTriggerEnter(Collider other) {

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
