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
    }
	

    void RotateHorizontal(Vector3 direction) {
        transform.Rotate(direction);
    }

    void RotateVertical(Vector3 direction, bool up) {
        Debug.Log(verticalAxis.eulerAngles.z);
        if (!up && verticalAxis.eulerAngles.z > 90f) verticalAxis.Rotate(direction);
        if (up && verticalAxis.eulerAngles.z < 160f) verticalAxis.Rotate(direction);
    }

    private void OnEnable() {
        // event subscription section
        // get the player object to subscribe
        if(aimUI == null) aimUI = FindObjectOfType<AimUI>();
        // subscribe to the launch toggle event
        aimUI.rotateHorizontalEvent += RotateHorizontal;
        // subscribe to initiate launch event
        aimUI.rotateVerticalEvent += RotateVertical;
    }

    void Unsubscribe() {
        // unsubscribe to the horizontal rotate event
        aimUI.rotateHorizontalEvent -= RotateHorizontal;
        // unsubscribe to vertical rotate event
        aimUI.rotateVerticalEvent -= RotateVertical;
    }

    private void OnDisable() {
        Unsubscribe();
    }

    private void OnDestroy() {
        Unsubscribe();
    }
}
