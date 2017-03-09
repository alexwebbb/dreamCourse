using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {

    // broadcast event for turn end
    public event Action endLaunchEvent;
    // for grabbing the event parent
    LaunchUI launchUI;


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


    Rigidbody playerObjectRB;

    int restCounter = 0;
    int restLimit = 150;

    bool launchModeBool;
    bool playerLaunchBool;
    bool resetBool;

    // experimental section
    [Header("Experimental Section")]
    public Vector3 centerOfMass;


    void Start() {

        // grab the players rigidbody for launching purposes
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        // grab the pointer cube that is used to direct the aim of the launcher. the launcher determines launch direction by using this cube and the player object
        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);
    }


    void FixedUpdate() {
        
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
                
                // a condition for the position reset
                resetBool = true;

                // stops the coroutine
                launchModeBool = false;
                
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
        GameObject testBall = traceBool ? (GameObject)Instantiate(tracerObject, playerObject.transform.position, playerObject.transform.localRotation) : playerObject;

        // weird fix for launching inside water bug and other issues
        if(!traceBool) {
            testBall.SetActive(false);
            testBall.SetActive(true);
        }

        // grabs the rigidbody of the object that is produced by the prior operation
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        // sets properties for the object, so you don't have to set properties in two different places. this section will likely be expanded. magnus force is a strong candidate
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        // experimental --- changing the center of mass 

        testballRB.centerOfMass = centerOfMass;

        

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

        // resets localRotation
        playerObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        // locks the player object in place
        playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;

        // pops the launcher over to the position of the player
        transform.position = playerObject.transform.position;

        // turns off the boolean that allows the position reset to occur in the update process
        resetBool = false;

        // call the event that signals to the rest of the system (namely the level controller) that the turn has ended.
        if (endLaunchEvent != null) endLaunchEvent();
    }

    // these are called by events
    // these are all simple functions for controlling the variables used in launch ingame

    public void SetForce(float _force) {
        force = _force;
    }

    public void SetMedialSpin(float _medialSpin) {
        spinForce.x = _medialSpin;
    }

    public void SetLateralSpin(float _lateralSpin) {
        spinForce.y = _lateralSpin;
    }

    // Functions called by the UI
    void ToggleLaunchMode() {
        // this begins the ball looper routine when L is pressed if it is not running, and ends it if it is
        if (!launchModeBool && !resetBool) {
            StartCoroutine("BallLooper");
        } else {
            playerLaunchBool = false;
            launchModeBool = false;
        }
    }

    void InitiateLaunchFromUI() {
        // this actually launches the ball, when space bar is pressed
        if (launchModeBool) {
            playerLaunchBool = true;
        }
    }

    void Subscribe() {
        // event subscription section
        // get the launch UI to subscribe to it
        launchUI = FindObjectOfType<LaunchUI>();
        // subscribe to the launch toggle event
        launchUI.launchToggleEvent += ToggleLaunchMode;
        // subscribe to initiate launch event
        launchUI.initiateLaunchEvent += InitiateLaunchFromUI;
        // subscribe to set force event
        launchUI.setForceEvent += SetForce;
        // subscribe to set medial spin event
        launchUI.setMedialSpinEvent += SetMedialSpin;
        // subscribe to set lateral spin
        launchUI.setLateralSpinEvent += SetLateralSpin;
    }

    void OnEnable() {
        Subscribe();
    }

    void Unsubscribe() {
        // unsubscribe to the launch toggle event
        launchUI.launchToggleEvent -= ToggleLaunchMode;
        // unsubscribe to initiate launch event
        launchUI.initiateLaunchEvent -= InitiateLaunchFromUI;
        // unsubscribe to set force event
        launchUI.setForceEvent -= SetForce;
        // unsubscribe to set medial spin event
        launchUI.setMedialSpinEvent -= SetMedialSpin;
        // unsubscribe to set lateral spin
        launchUI.setLateralSpinEvent -= SetLateralSpin;
    }

    void OnDestroy() {
        Unsubscribe();
    }

    void OnDisable() {
        Unsubscribe();
    }
}
