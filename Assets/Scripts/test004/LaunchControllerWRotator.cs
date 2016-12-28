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

    int restCounter = 0;
    int restLimit = 150;

    bool launchModeBool;
    bool playerLaunchBool;
    bool resetBool;


    void Start() {
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            if (!launchModeBool) {
                StartCoroutine("BallLooper");
            } else {
                playerLaunchBool = false;
                launchModeBool = false;
            }
        }

        if (launchModeBool && Input.GetKeyDown(KeyCode.Space)) {
            playerLaunchBool = true;
        }

        if (resetBool && !launchModeBool && playerObjectRB.angularVelocity.sqrMagnitude < rollLimit && playerObjectRB.velocity.sqrMagnitude < velocitySleep) {

            restCounter++;

            if (restCounter > restLimit) {
                
                PositionReset();
                restCounter = 0;
            }
        }
           

    }

    IEnumerator BallLooper() {

        launchModeBool = true;
        while (launchModeBool) {

            launchDirection = pointerCube.transform.position - transform.position;

            if (playerLaunchBool) {
                // launch player, end coroutine
                playerObjectRB.constraints = RigidbodyConstraints.None;
                Launch(false);
                playerLaunchBool = false;
                launchModeBool = false;
                resetBool = true;

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

        testballRB.AddRelativeForce(launchDirection * force, ForceMode.Impulse);
        testballRB.AddTorque(spinForce);

        if (traceBool) Destroy(testBall, lifetime);
    }

    void PositionReset() {
        playerObjectRB.angularVelocity = playerObjectRB.velocity = Vector3.zero;
        playerObject.transform.rotation = Quaternion.identity;
        transform.position = playerObject.transform.position;
        playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;
        resetBool = false;
    }

}
