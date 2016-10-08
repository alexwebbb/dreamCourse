using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public VirtualJoystick joystick;

    void Start() {
        transform.rotation = player.transform.rotation;
    }

    void FixedUpdate() {

        transform.position = player.transform.position;
        transform.Rotate(joystick.Vertical(), joystick.Horizontal(), 0);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

    }
}
