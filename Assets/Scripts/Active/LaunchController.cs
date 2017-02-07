using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {


    public GameObject tracerObject;
    public GameObject playerObject;

    Transform pointerCube;


    [Header("Bounce Controller Items")]
    public Vector3 spinForce;
    public float spinForceFactor = 0.05f;
    public float bouncePercent = 0.50f;

    [Header("Launch Components")]
    public float force;
    public float lifetime;
    public float ballGap;

    [Header("Rolling Characteristics")]
    public float angleDrag;
    public float drag;
    public float rollLimit = 5f;
    public float velocitySleep = 5f;

    /* These events may be used later for UI controls
    public event Action launchModeBegin;
    public event Action launchModeEnd;
    */

    Rigidbody playerObjectRB;

    int restCounter = 0;
    int restLimit = 150;

    bool launchModeBool;
    bool playerLaunchBool;
    bool resetBool;


    void Start() {

        // grab the players rigidbody for launching purposes
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        // grab the pointer cube that is used to direct the aim of the launcher. the launcher determines launch direction by using this cube and the player object
        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);
    }


    void Update() {

        // this begins the ball looper routine when L is pressed if it is not running, and ends it if it is
        if (Input.GetKeyDown(KeyCode.L)) {
            if (!launchModeBool) {
                StartCoroutine("BallLooper");
            } else {
                playerLaunchBool = false;
                launchModeBool = false;
            }
        }


        // this actually launches the ball, when space bar is pressed
        if (launchModeBool && Input.GetKeyDown(KeyCode.Space)) {
            playerLaunchBool = true;
        }

        // this resets the position of the ball when it gets close enough to not moving
        if (resetBool && !launchModeBool && playerObjectRB.angularVelocity.sqrMagnitude < rollLimit && playerObjectRB.velocity.sqrMagnitude < velocitySleep) {

            // this increments the rest counter each time the condition above is satisfied... thus you can set it to whatever amount of time you want the ball to be in a resting state before it resets. this is important for when it is balanced on edges and it might take a second for it to fall off
            restCounter++;

            // this resets the bounce counter that is attached to the player
            playerObject.GetComponent<BounceController>().bounceCount = 0;

            // this is where the rest limit is checked
            if (restCounter > restLimit) {
                
                // the player object is reset and the rest counter is reset
                PositionReset();
                restCounter = 0;
            }
        }
           

    }

    IEnumerator BallLooper() {

        launchModeBool = true;
        while (launchModeBool) {

            // launch player, end coroutine
            if (playerLaunchBool) {

                // unlocks the player object constraints 
                playerObjectRB.constraints = RigidbodyConstraints.None;
                
                // launches the player.... the boolean represents whether the tracer will be used or not... false indicates the player
                Launch(false);
                
                // turns off the player launch trigger now that it has been executed
                playerLaunchBool = false;
                
                // stops the coroutine
                launchModeBool = false;
                
                // a condition for the position reset
                resetBool = true;
            } else {
                
                // launch tracer
                Launch(true);
            }

            //
            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }

    void Launch(bool traceBool) {

        // this ternary operator uses the trace bool to determine whether to launch a tracer object or the player object
        GameObject testBall = traceBool ? (GameObject)Instantiate(tracerObject, playerObject.transform.position, playerObject.transform.rotation) : playerObject;

        // grabs the rigidbody of the object that is produced by the prior operation
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        // sets properties for the object, so you don't have to set properties in two different places. this section will likely be expanded. magnus force is a strong candidate
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        // the object which has been grabbed is pointed at the cube
        testballRB.transform.LookAt(pointerCube);

        // launch forces are applied
        testballRB.AddRelativeForce(Vector3.forward * force, ForceMode.VelocityChange);
        testballRB.AddRelativeTorque(spinForce, ForceMode.VelocityChange);

        // sets the decay time for the tracer objects
        if (traceBool) Destroy(testBall, lifetime);
    }

    void PositionReset() {

        // stops movement
        playerObjectRB.angularVelocity = playerObjectRB.velocity = Vector3.zero;

        // resets rotation
        playerObject.transform.rotation = Quaternion.identity;

        // locks the player object in place
        playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;

        // pops the launcher over to the position of the player
        transform.position = playerObject.transform.position;

        // turns off the boolean that allows the position reset to occur in the update process
        resetBool = false;
    }


    // these are all simple functions for controlling the variables used in launch ingame

    public void setForce(float _force) {
        force = _force;
    }

    public void setMedialSpin(float _medialSpin) {
        spinForce.x = _medialSpin;
    }

    public void setLateralSpin(float _lateralSpin) {
        spinForce.y = _lateralSpin;
    }
}
