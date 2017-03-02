using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AimUI : MonoBehaviour {

    public event Action<Vector3> rotateHorizontalEvent;
    public event Action<Vector3, bool> rotateVerticalEvent;

    public float movementInterval = 15;
    public float heldButtonInterval = 0.15f;

    Vector3 left;
    Vector3 right;
    Vector3 down;
    Vector3 up;

    float heldTime;

    void Start() {

        // default movement interval is assigned to shortcut variables
        // will need to change how this works... perhaps an editor button
        left.y = -movementInterval;
        right.y = movementInterval;
        down.z = -movementInterval;
        up.z = movementInterval;
    }

    void Update() {

        heldTime = heldTime + Time.deltaTime;

        if (rotateHorizontalEvent != null) {
            // rotates this transform horizongtally	
            if ((Input.GetAxisRaw("Horizontal") < -0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                // rotate left
                rotateHorizontalEvent(left);
                heldTime = 0.0f;
            } else if ((Input.GetAxisRaw("Horizontal") > 0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                // rotate right
                rotateHorizontalEvent(right);
                heldTime = 0.0f;
            }
        }

        if (rotateVerticalEvent != null) {
            // rotates this transform vertically. The limits are hard coded, this could perhaps change
            if ((Input.GetAxisRaw("Vertical") < -0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                // rotate down
                rotateVerticalEvent(down, false);
                heldTime = 0.0f;
            } else if ((Input.GetAxisRaw("Vertical") > 0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                // rotate up
                rotateVerticalEvent(up, true);
                heldTime = 0.0f;
            } 
        } 
    }
}
