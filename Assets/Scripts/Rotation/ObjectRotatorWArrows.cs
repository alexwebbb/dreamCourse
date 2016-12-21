using UnityEngine;
using System.Collections;

public class ObjectRotatorWArrows : MonoBehaviour {

    public float movementInterval = 15;

    Transform verticalAxis;
    Vector3 left;
    Vector3 right;
    Vector3 down;
    Vector3 up;

	// Use this for initialization
	void Start () {
        left.y = -movementInterval;
        right.y = movementInterval;
        down.z = -movementInterval;
        up.z = movementInterval;

        verticalAxis = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.Rotate(left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.Rotate(right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if(verticalAxis.eulerAngles.z > 10f) verticalAxis.Rotate(down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if(verticalAxis.eulerAngles.z < 70f) verticalAxis.Rotate(up);
        }

    }
}
