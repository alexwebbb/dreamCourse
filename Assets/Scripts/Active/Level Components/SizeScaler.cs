using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleType { Static, Linear, Radial };

public class SizeScaler : MonoBehaviour {

    public float targetSize;

    public ScaleType type = ScaleType.Radial; 

    private void OnTriggerEnter(Collider other) {
        ConstantScale cs = other.GetComponent<ConstantScale>();
        if (cs != null) cs.SetReference = this;
    }

    private void OnTriggerExit(Collider other) {
        ConstantScale cs = other.GetComponent<ConstantScale>();
        if (cs != null) cs.UnsetReference = this;
    }
}