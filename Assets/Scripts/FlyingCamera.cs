using UnityEngine;
using System.Collections;

public class FlyingCamera : MonoBehaviour {

    public GameObject player;
    public VirtualJoystick joystick1;
    public VirtualJoystick joystick2;

    public float scalar = 0.1f; 

    void Start() {
        transform.rotation = player.transform.rotation;
    }

    void FixedUpdate() {

        transform.position = player.transform.position;
        transform.Rotate(joystick1.Vertical(), joystick1.Horizontal(), 0);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        // zooming in and out by scaling camera container
        transform.localScale += new Vector3(joystick2.Vertical() * scalar, joystick2.Vertical() * scalar, joystick2.Vertical() * scalar);

        // Clamps range of zoom
        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.5f, 10f), Mathf.Clamp(transform.localScale.y, 0.5f, 10f), Mathf.Clamp(transform.localScale.z, 0.5f, 10f));


    }
}
