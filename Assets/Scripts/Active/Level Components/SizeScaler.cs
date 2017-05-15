using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleType { Static, Linear, Radial };

public class SizeScaler : MonoBehaviour {

    public float targetSize;

    public ScaleType type; 

    private void Start() {
        
    }

    private void FixedUpdate() {
        // check if resident constant scale objects are available to be set
        // there is a problem with this, ie what to do about overlapping fields
        // this was intended to be a fix for that... but it doesnt actually solve that problem
        // maybe it will work after all. since radial field only cares about 
        // your initial scale, not how far you are from the center when it activates
    }

    private void OnTriggerEnter(Collider other) {
        // adds constant scale object to list which is checked in the fixed update
    }

    private void OnTriggerExit(Collider other) {
        // unsets the object exiting the field, removes it from the list
    }
}