using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public VirtualJoystick joystick;
    public Transform camTransform;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {

        float moveHorizontal = joystick.Horizontal();
        float moveVertical = joystick.Vertical();

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement.magnitude > 1) movement.Normalize();

        Vector3 dir = camTransform.TransformDirection(movement);

        dir.Set(dir.x, 0, dir.z);
        movement = dir.normalized * movement.magnitude;

        rb.AddForce(movement * speed);
    }
}
