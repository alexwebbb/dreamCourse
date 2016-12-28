using UnityEngine;
using System.Collections;

public class FlyingCamera : MonoBehaviour {

    public GameObject player;
    public VirtualJoystick joystick1;
    public VirtualJoystick joystick2;

    public float orthoSize;
    public float scalar = 0.1f;
    public float defaultHeight = 5;

    Camera mainCamera;

    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.orthographicSize = orthoSize;
        transform.rotation = player.transform.rotation;
    }

    void FixedUpdate() {

        transform.position = player.transform.position;

        // right joystickk control. this is control for rotating the camera
        transform.Rotate(joystick1.Vertical(), joystick1.Horizontal(), 0);
        transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        // zooming in and out by scaling camera container
        transform.localScale += new Vector3(joystick1.Vertical() * scalar, joystick1.Vertical() * scalar, joystick1.Vertical() * scalar);

        // Clamps range of zoom
        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.5f, 10f), Mathf.Clamp(transform.localScale.y, 0.5f, 10f), Mathf.Clamp(transform.localScale.z, 0.5f, 10f));

        // 
        
    }

    public void setAngleOfView(float viewAngle) {
        mainCamera.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);
        mainCamera.transform.localPosition = new Vector3(0, viewAngle - defaultHeight, 0);
    }

    public void setOrthoSize(float _orthoSize) {
        mainCamera.orthographicSize = _orthoSize;
        mainCamera.transform.localPosition = new Vector3(0, _orthoSize - defaultHeight, 0);
    }


}
