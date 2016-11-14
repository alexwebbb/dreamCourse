using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {

    public GameObject tracerObject;
    public GameObject playerObject;

    public float force;
    public Vector3 spinForce;
    public Vector3 launchDirection;
    public float lifetime;
    public float angleDrag;
    public float drag;
    public float rollLimit = 5f;
    public float velocitySleep = 5f;
    public float ballGap;

    public event Action launchModeBegin;
    public event Action launchModeEnd;

    Rigidbody rb;

    bool launchBool;
    bool playerLaunchBool;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            if (!launchBool) {
                StartCoroutine("BallLooper");
                if (launchModeBegin != null) launchModeBegin();
            } else {
                if (launchModeBegin != null) launchModeEnd();
                playerLaunchBool = false;
                launchBool = false;
            }
        }

        if (launchBool && Input.GetKeyDown(KeyCode.Space)) playerLaunchBool = true;

        if (rb.angularVelocity.sqrMagnitude < rollLimit && rb.velocity.sqrMagnitude < velocitySleep) PositionReset();

    }

    IEnumerator BallLooper() {

        launchBool = true;
        while (launchBool) {
            
            if(playerLaunchBool) {
                // launch player, end coroutine
                Launch(playerObject, false);
                playerLaunchBool = false;
                launchBool = false;

            } else {
                // launch tracer
                Launch(tracerObject, true);
            }

            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }

    void Launch(GameObject missile, bool traceBool) {

        GameObject testBall = traceBool ? (GameObject)Instantiate(missile, playerObject.transform.position, transform.rotation) : playerObject;
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        testballRB.maxAngularVelocity = 1000f;
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        testballRB.AddRelativeForce(launchDirection * force, ForceMode.Impulse);
        testballRB.AddTorque(spinForce);
        

        if(traceBool) Destroy(testBall, lifetime);
    }

    void PositionReset() {
        rb.angularVelocity = rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.identity, 2f * Time.time);
    }


}
