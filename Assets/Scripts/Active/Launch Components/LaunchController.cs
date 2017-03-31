using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {

    // broadcast event for turn end
    // public event Action ballRestingEvent;
    // public event Action<bool> ballReadyEvent;
    // public event Action<bool> launchModeActiveEvent;

    // for grabbing the event parent
    LevelController levelController;
    LaunchUI launchUI;

    public GameObject tracerObject;
    public GameObject playerObject;

    Transform pointerCube;

    [Header("Bounce Controller Items")]
    public Vector3 spinForce;
    public float spinForceFactor = 0.05f;
    public float bouncePercent = 0.50f;

    [Header("Launch Components")]
    public float defaultForce;
    public float minForce;
    public float maxForce;

    float normalizedForce;
    float GetPlayerForce {
        get {
            return (normalizedForce * (maxForce - minForce)) + minForce;
        }
    }

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

    bool launchModeIsActive;
    bool playerLaunchButtonHasBeenPressed;
    bool readyToRest;
    bool launchInitiated;
    bool playerReadyForLaunch;

    // experimental section
    [Header("Experimental Section")]
    public Vector3 centerOfMass;

    void Awake() {
        // grab the players rigidbody for launching purposes
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        // grab the pointer cube that is used to direct the aim of the launcher. the launcher determines launch direction by using this cube and the player object
        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);

        // get the launch UI to subscribe to it
        launchUI = FindObjectOfType<LaunchUI>();

        // register the launch controller with the launch UI for launch events, such as resetting values
        // will probably remove this
        launchUI.RegisterLauncher(this);
        // fetch the level controller
        levelController = FindObjectOfType<LevelController>();
    }


    void FixedUpdate() {
        // this block is here in case the player hasn't moved at all.. so that it can skip that few second count down
        if(!playerReadyForLaunch && !launchInitiated && playerObjectRB.transform.position == transform.position) {
            // immediately turn off the variable that passed this block, so that begin turn is called only once
            playerReadyForLaunch = true;
            // set rest bool to false... needs to happen before begin turn is called
            ResetRestCheck();
            // begin turn event... this will be used for UI and such also
            levelController.BeginTurn(false);
        } 

        // this resets the position of the ball when it gets close enough to not moving
        if (readyToRest && !launchModeIsActive && playerObjectRB.angularVelocity.sqrMagnitude < rollLimit && playerObjectRB.velocity.sqrMagnitude < velocitySleep) {
            // this increments the rest counter each time the condition above is satisfied
            // you can set it to whatever amount of time you want the ball to be in a resting state before it resets.
            // this is important for when it is balanced on edges and it might take a second for it to fall off
            restCounter++;

            // this is where the rest limit is checked
            if (restCounter > restLimit) {
                playerReadyForLaunch = true;
                // clear the variables for sleeping after calling begin turn or end turn, it is also called on disable which activates in multiplayer mode
                ResetRestCheck();

                if (launchInitiated) {
                    // turn this off so that end turn event doesn't get called twice
                    launchInitiated = false;
                    // call the end turn event in the levelcontroller.. most player switching logic is there
                    levelController.EndTurn();
                    
                } else {
                    // lock position at beginning of turn
                    playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;
                    // call this at the beginning of the turn in case the player is rolling along as a result of other player action or hazards
                    levelController.BeginTurn(false);
                }
            }
        }
    }


    void ResetRestCheck() {
        // these are the variables used in the fixed update to allow the launch controller to sleep the ball
        readyToRest = false;
        // rest counter is reset
        restCounter = 0;
    }


    IEnumerator BallLooper() {

        launchModeIsActive = true;

        while (launchModeIsActive) {

            // launch player, end coroutine
            if (playerLaunchButtonHasBeenPressed) {
                // turns off the player launch trigger now that it has been executed
                playerLaunchButtonHasBeenPressed = false;
                // now that we are launching, the player is no longer ready to go
                playerReadyForLaunch = false;
                // the launch has been initiated. here we are setting related booleans
                launchInitiated = true;
                // a condition for the position reset at the end of the turn
                readyToRest = true;
                // would stop the loop, if the yield below didnt already do that, but it is important for the state
                launchModeIsActive = false;
                // unlocks the player object constraints
                playerObjectRB.constraints = RigidbodyConstraints.None;

                // launches the player. false indicates the player
                Launch(false);
                // exit the coroutine altogether
                yield return null;

            } else {   
                // launch tracer block and loop back in the while loop
                Launch(true);
            }
            // this sets the space between the tracer objects
            yield return new WaitForSeconds(ballGap);
        }
        yield return null;
    }

    void Launch(bool traceBool) {

        // this ternary operator uses the trace bool to determine whether to launch a tracer object or the player object
        GameObject launchingObject = traceBool ? (GameObject)Instantiate(tracerObject, playerObject.transform.position, playerObject.transform.localRotation) : playerObject;
        
        // get the launching object's bounce controller so that it can invoke the set launcher property
        BounceController bc = launchingObject.GetComponent<BounceController>();
        // set launcher checks to see if the launch controller has been set (ie the player object) and sets initial variables
        bc.SetLauncher = this;

        // for launching inside water, so that ontrigger enter will be called, etc
        if(!traceBool) { launchingObject.SetActive(false); launchingObject.SetActive(true);}
        
        // grabs the rigidbody of tahe object that is produced by the prior operation
        Rigidbody launchingRB = bc.GetRigidbody;

        // the object which has been grabbed is pointed at the cube
        launchingRB.transform.LookAt(pointerCube);

        // launch forces are applied
        if(traceBool) launchingRB.AddRelativeForce(Vector3.forward * defaultForce, ForceMode.VelocityChange);
        else launchingRB.AddRelativeForce(Vector3.forward * GetPlayerForce, ForceMode.VelocityChange);
        launchingRB.AddRelativeTorque(spinForce, ForceMode.VelocityChange);

        // sets the decay time for the tracer objects
        if (traceBool) Destroy(launchingObject, lifetime);
    }

    // Functions called by the UI
    void ToggleLaunchMode() {
        // this begins the ball looper routine when L is pressed if it is not running, and ends it if it is
        if (!launchModeIsActive && !readyToRest) {
            StartCoroutine("BallLooper");
        } else {
            launchModeIsActive = false;
        }
    }

    void InitiateLaunchFromUI() {
        // this actually launches the ball, when space bar is pressed
        if (launchModeIsActive) playerLaunchButtonHasBeenPressed = true;
    }

    void Subscribe() {
        // subscribe to the launch toggle event
        launchUI.launchModeToggleEvent += ToggleLaunchMode;
        // subscribe to initiate launch event
        launchUI.initiateLaunchEvent += InitiateLaunchFromUI;
        // subscribe to set force event
        launchUI.setForceEvent += SetForce;
        // subscribe to set medial spin event
        launchUI.setMedialSpinEvent += SetMedialSpin;
        // subscribe to set lateral spin
        launchUI.setLateralSpinEvent += SetLateralSpin;
    }

    void Unsubscribe() {
        // unsubscribe to the launch toggle event
        launchUI.launchModeToggleEvent -= ToggleLaunchMode;
        // unsubscribe to initiate launch event
        launchUI.initiateLaunchEvent -= InitiateLaunchFromUI;
        // unsubscribe to set force event
        launchUI.setForceEvent -= SetForce;
        // unsubscribe to set medial spin event
        launchUI.setMedialSpinEvent -= SetMedialSpin;
        // unsubscribe to set lateral spin
        launchUI.setLateralSpinEvent -= SetLateralSpin;
    }

    void ResetLaunchValues() {
        // this covers both later and medial spin
        spinForce = Vector3.zero;
    }

    void OnEnable() {
        // subscribe to all the UI events
        Subscribe();
        // the ball is ready to rest now that its turn is now active
        readyToRest = true;
    }

    void OnDestroy() {
        Unsubscribe();
    }

    void OnDisable() {
        // reset values set by UI during launch
        ResetLaunchValues();

        // unsubscribe the events so it doesn't call an error while inactive
        Unsubscribe();

        // clear the variables used for sleeping the player object
        ResetRestCheck();

        launchInitiated = false;
    }


    // these are called by events

    public void SetForce(float _normalizedForce) {
        normalizedForce = _normalizedForce;
    }

    public void SetMedialSpin(float _medialSpin) {
        spinForce.x = _medialSpin;
    }

    public void SetLateralSpin(float _lateralSpin) {
        spinForce.y = _lateralSpin;
    }

}


