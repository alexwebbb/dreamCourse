using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour {

    public float movementInterval = 15;

    Transform verticalAxis;
    Vector3 left;
    Vector3 right;
    Vector3 down;
    Vector3 up;

	void Start () {

        // default movement interval is assigned to shortcut variables
        left.y = -movementInterval;
        right.y = movementInterval;
        down.z = -movementInterval;
        up.z = movementInterval;

        // a seperate transform is used for the vertical access to avoid gimble lock
        verticalAxis = transform.GetChild(0);
	}
	
	void Update () {


        // rotates this transform horizongtally	
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.Rotate(left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.Rotate(right);
        }

        // rotates this transform vertically. The limits are hard coded, this could perhaps change
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if(verticalAxis.eulerAngles.z > 10f) verticalAxis.Rotate(down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if(verticalAxis.eulerAngles.z < 70f) verticalAxis.Rotate(up);
        }

    }
}
