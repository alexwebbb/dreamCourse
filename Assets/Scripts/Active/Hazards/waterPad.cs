using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPad : MonoBehaviour {

    public float power = 5f;
    public float resistance = 2f;

    LaunchController lc;

    private void Start() {
        lc = GameObject.FindGameObjectWithTag("Launcher").GetComponent<LaunchController>();
    }

    private void OnTriggerEnter(Collider other) {

        other.GetComponent<Rigidbody>().drag = lc.drag + resistance;

        ConstantForce cf = other.gameObject.GetComponent<ConstantForce>();

        cf.force = new Vector3(0, power, 0);


    }

    private void OnTriggerExit(Collider other) {

        other.GetComponent<Rigidbody>().drag = lc.drag;

        ConstantForce cf = other.gameObject.GetComponent<ConstantForce>();

        cf.force = Vector3.zero;

    }
}
