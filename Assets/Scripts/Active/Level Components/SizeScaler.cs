using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour {

    public float minSize;
    public float maxSize;

    List<GameObject> captiveObject = new List<GameObject>();

    private void Start() {
        
    }

    private void FixedUpdate() {
        foreach(GameObject captive in captiveObject) {
            if (captive != null) {
                float scalar = (captive.transform.position.z - (transform.position.z - (0.5f * transform.localScale.z))) / transform.localScale.z;
                scalar = (scalar * (maxSize - minSize)) + minSize;
                captive.transform.localScale = new Vector3(scalar, scalar, scalar);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 9 || other.gameObject.layer == 8) {
            captiveObject.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        captiveObject.Remove(other.gameObject);
    }
}
