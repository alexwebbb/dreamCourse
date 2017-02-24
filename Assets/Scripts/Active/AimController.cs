using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour {

    public float movementInterval = 15;
    public float heldButtonInterval = 0.05f;

    Transform verticalAxis;
    Vector3 left;
    Vector3 right;
    Vector3 down;
    Vector3 up;

    float heldTime;

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


        heldTime = heldTime + Time.deltaTime;

        // rotates this transform horizongtally	
        if ((Input.GetAxisRaw("Horizontal") < -0.5f  && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            transform.Rotate(left);
            heldTime = 0.0f;
        } else if ((Input.GetAxisRaw("Horizontal") > 0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            transform.Rotate(right);
            heldTime = 0.0f;
        }

        // rotates this transform vertically. The limits are hard coded, this could perhaps change
        if ((Input.GetAxisRaw("Vertical") < -0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if(verticalAxis.eulerAngles.z > 10f) verticalAxis.Rotate(down);
            heldTime = 0.0f;
        } else if ((Input.GetAxisRaw("Vertical") > 0.5f && heldTime > heldButtonInterval) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            if(verticalAxis.eulerAngles.z < 70f) verticalAxis.Rotate(up);
            heldTime = 0.0f;
        }
    }
}
