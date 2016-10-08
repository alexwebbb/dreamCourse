using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpPower = 10;
    public VirtualJoystick joystick;
    public Transform camTransform;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {

        // Joystick Input
        float moveHorizontal = joystick.Horizontal();
        float moveVertical = joystick.Vertical();

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement.magnitude > 1) movement.Normalize();

        Vector3 dir = camTransform.TransformDirection(movement);

        dir.Set(dir.x, 0, dir.z);
        movement = dir.normalized * movement.magnitude;

        // Player movement
        rb.AddForce(movement * speed, ForceMode.Force);

        // jump mechanics

        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(new Vector3(0,1,0) * jumpPower, ForceMode.Impulse);
            
        }

    }
}
