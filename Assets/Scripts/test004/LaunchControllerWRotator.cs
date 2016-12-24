using UnityEngine;
using System;
using System.Collections;

public class LaunchControllerWRotator : MonoBehaviour {

    public GameObject tracerObject;
    public GameObject playerObject;

    Transform pointerCube;

    Vector3 launchDirection;

    public float force;
    public Vector3 spinForce;
    public float lifetime;
    public float angleDrag;
    public float drag;
    public float rollLimit = 5f;
    public float velocitySleep = 5f;
    public float ballGap;

    public event Action launchModeBegin;
    public event Action launchModeEnd;

    Rigidbody playerObjectRB;

    bool launchBool;
    bool playerLaunchBool;


    void Start() {
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            if (!launchBool) {
                StartCoroutine("BallLooper");
            } else {
                playerLaunchBool = false;
                launchBool = false;
            }
        }

        if (launchBool && Input.GetKeyDown(KeyCode.Space)) {
            playerLaunchBool = true;
        }

        // if (!launchBool && playerObjectRB.angularVelocity.sqrMagnitude < rollLimit && playerObjectRB.velocity.sqrMagnitude < velocitySleep) PositionReset();
           

    }

    IEnumerator BallLooper() {

        launchBool = true;
        while (launchBool) {

            launchDirection = pointerCube.transform.position - transform.position;

            if (playerLaunchBool) {
                // launch player, end coroutine
                Launch(false);
                playerLaunchBool = false;
                launchBool = false;

            } else {
                // launch tracer
                Launch(true);
            }

            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }

    void Launch(bool traceBool) {

        GameObject testBall = traceBool ? (GameObject)Instantiate(tracerObject, playerObject.transform.position, Quaternion.identity) : playerObject;
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        testballRB.maxAngularVelocity = 1000f;
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        Debug.Log(launchDirection);

        testballRB.AddRelativeForce(launchDirection * force, ForceMode.Impulse);
        testballRB.AddTorque(spinForce);

        if (traceBool) Destroy(testBall, lifetime);
    }

    void PositionReset() {
        playerObjectRB.angularVelocity = playerObjectRB.velocity = Vector3.zero;
        playerObject.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.position = playerObject.transform.position;
    }

}
