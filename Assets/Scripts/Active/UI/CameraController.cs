using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public VirtualJoystick joystick1;
    // public Transform cameraTransform;

    public float scalar = 0.1f;
    public float defaultHeight = 5;

    void Start() {

        // ensures that the camera is pointing in the same direction as the player at the start.
        transform.forward = player.transform.parent.forward;

    }

    void FixedUpdate() {

        // follows the object placed in the player position
        transform.position = player.transform.position;

        // right joystickk control. this is control for rotating the camera
        transform.Rotate(joystick1.Vertical(), joystick1.Horizontal(), 0);
        transform.localRotation = Quaternion.Euler( transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0);

        // zooming in and out by scaling camera container
        transform.localScale += new Vector3(joystick1.Vertical() * scalar, joystick1.Vertical() * scalar, joystick1.Vertical() * scalar);

        // Clamps range of zoom
        transform.localScale = new Vector3(Mathf.Clamp( transform.localScale.x, 0.5f, 10f), Mathf.Clamp( transform.localScale.y, 0.5f, 10f), Mathf.Clamp( transform.localScale.z, 0.5f, 10f));   
    }
}
