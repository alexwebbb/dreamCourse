using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public VirtualJoystick joystick1;
    public Transform cameraTransform;

    public float scalar = 0.1f;
    public float defaultHeight = 5;

    void Start() {

        // ensures that the camera is pointing in the same direction as the player at the start.
        cameraTransform.forward = player.transform.parent.forward;

    }

    void FixedUpdate() {

        // follows the object placed in the player position
        cameraTransform.position = player.transform.position;

        // right joystickk control. this is control for rotating the camera
        cameraTransform.Rotate(joystick1.Vertical(), joystick1.Horizontal(), 0);
        cameraTransform.localRotation = Quaternion.Euler( cameraTransform.localRotation.eulerAngles.x, cameraTransform.localRotation.eulerAngles.y, 0);

        // zooming in and out by scaling camera container
        cameraTransform.localScale += new Vector3(joystick1.Vertical() * scalar, joystick1.Vertical() * scalar, joystick1.Vertical() * scalar);

        // Clamps range of zoom
        cameraTransform.localScale = new Vector3(Mathf.Clamp( cameraTransform.localScale.x, 0.5f, 10f), Mathf.Clamp( cameraTransform.localScale.y, 0.5f, 10f), Mathf.Clamp( cameraTransform.localScale.z, 0.5f, 10f));   
    }
}
