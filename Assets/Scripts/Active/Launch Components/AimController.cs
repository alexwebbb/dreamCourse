using UnityEngine;
using System;
using System.Collections;

public class AimController : MonoBehaviour {

    // for grabbing the event parent
    AimUI aimUI;

    Transform verticalAxis;

	void Start () {

        // a seperate transform is used for the vertical access to avoid gimble lock
        verticalAxis = transform.GetChild(0);

        // event subscription section
        // get the player object to subscribe
        aimUI = FindObjectOfType<AimUI>();
        // subscribe to the launch toggle event
        aimUI.rotateHorizontalEvent += RotateHorizontal;
        // subscribe to initiate launch event
        aimUI.rotateVerticalEvent += RotateVertical;
    }
	

    void RotateHorizontal(Vector3 direction) {
        transform.Rotate(direction);
    }

    void RotateVertical(Vector3 direction, bool up) {

        if (!up && verticalAxis.eulerAngles.z > 10f) verticalAxis.Rotate(direction);
        if (up && verticalAxis.eulerAngles.z < 70f) verticalAxis.Rotate(direction);
    }

    void OnDestroy() {
        // unsubscribe to the launch toggle event
        aimUI.rotateHorizontalEvent -= RotateHorizontal;
        // unsubscribe to initiate launch event
        aimUI.rotateVerticalEvent -= RotateVertical;
    }
}
