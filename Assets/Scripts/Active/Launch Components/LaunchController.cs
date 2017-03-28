﻿using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {

    // broadcast event for turn end
    public event Action endTurnEvent;
    public event Action<bool> beginTurnEvent;
    public event Action startLaunchModeEvent;
    public event Action stopLaunchModeEvent;
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
    public float defaultForce;
    float playerForce;
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
    bool restBool;
    bool turnInitiated;

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
        launchUI.RegisterLauncher(this);
    }


    void FixedUpdate() {

        // Debug.Log("Rest Bool : " + restBool);
        // Debug.Log("Turn Initiated : " + turnInitiated);

        if(!turnInitiated && playerObjectRB.transform.position == transform.position) {
            // set rest bool to false... needs to happen before begin turn is called
            ClearSleepCheck();
            // begin turn event... this will be used for UI and such also
            if (beginTurnEvent != null) beginTurnEvent(restBool);
        } else {
            // Debug.Log("no match");
        }

        // this resets the position of the ball when it gets close enough to not moving
        if (restBool && !launchModeBool && playerObjectRB.angularVelocity.sqrMagnitude < rollLimit && playerObjectRB.velocity.sqrMagnitude < velocitySleep) {

            // this increments the rest counter each time the condition above is satisfied... thus you can set it to whatever amount of time you want the ball to be in a resting state before it resets. this is important for when it is balanced on edges and it might take a second for it to fall off
            restCounter++;

            // this is where the rest limit is checked
            if (restCounter > restLimit) {

                if (turnInitiated) {
                    // call the event that signals to the rest of the system (namely the level controller) that the turn has ended. this will trigger the position reset
                    if (endTurnEvent != null) endTurnEvent();

                    turnInitiated = false;
                } else {
                    // call this at the beginning of the turn in case the player is rolling along as a result of other player action or hazards
                    if (beginTurnEvent != null) beginTurnEvent(restBool);
                    // lock position at beginning of turn
                    playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;
                }
                // clear the variables for sleeping after calling begin turn or end turn, it is also called on disable which activates in multiplayer mode
                ClearSleepCheck();
            }
        }
    }


    void ClearSleepCheck() {
        // these are the variables used in the fixed update to allow the launch controller to sleep the ball. it is necessary to clear these before ending the turn. this gets called on disable and end turn involves disabling this sooo...

        // turns off the boolean that allows the position reset to occur in the update process
        restBool = false;

        // rest counter is reset
        restCounter = 0;
    }

    IEnumerator BallLooper() {

        // signal the start of launch mode to the system
        if (startLaunchModeEvent != null) startLaunchModeEvent();

        launchModeBool = true;
        while (launchModeBool) {

            // launch player, end coroutine
            if (playerLaunchBool) {

                // may still keep this in case of player effects that freeze position, like porcupine
                // unlocks the player object constraints
                playerObjectRB.constraints = RigidbodyConstraints.None;

                // launches the player.... the boolean represents whether the tracer will be used or not... false indicates the player
                Launch(false);
                
                // turns off the player launch trigger now that it has been executed
                playerLaunchBool = false;

                turnInitiated = true;

                // a condition for the position reset at the end of the turn
                restBool = true;

                // stops the coroutine
                launchModeBool = false;
                
            } else {
                   
                // launch tracer
                Launch(true);
            }
            // this sets the space between the tracer objects
            yield return new WaitForSeconds(ballGap);
        }

        // signal the end of launch mode to the system
        if (stopLaunchModeEvent != null) stopLaunchModeEvent();

        yield return null;
    }

    void Launch(bool traceBool) {

        // this ternary operator uses the trace bool to determine whether to launch a tracer object or the player object
        GameObject launchingObject = traceBool ? (GameObject)Instantiate(tracerObject, playerObject.transform.position, playerObject.transform.localRotation) : playerObject;
        
        // get the launching object's bounce controller so that it can invoke the set launcher property
        BounceController bc = launchingObject.GetComponent<BounceController>();
        // set launcher checks to see if the launch controller has been set (ie the player object) and sets initial variables
        bc.SetLauncher = this;

        // weird fix for launching inside water bug and other issues
        if(!traceBool) { launchingObject.SetActive(false); launchingObject.SetActive(true);}
        
        // grabs the rigidbody of the object that is produced by the prior operation
        Rigidbody launchingRB = bc.GetRigidbody;

        // the object which has been grabbed is pointed at the cube
        launchingRB.transform.LookAt(pointerCube);

        // launch forces are applied
        if(traceBool) launchingRB.AddRelativeForce(Vector3.forward * defaultForce, ForceMode.VelocityChange);
        else launchingRB.AddRelativeForce(Vector3.forward * playerForce, ForceMode.VelocityChange);
        launchingRB.AddRelativeTorque(spinForce, ForceMode.VelocityChange);

        // sets the decay time for the tracer objects
        if (traceBool) Destroy(launchingObject, lifetime);
    }

    // these are called by events
    // these are all simple functions for controlling the variables used in launch ingame

    public void SetForce(float _force) {
        playerForce = _force;
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
        if (!launchModeBool && !restBool) {
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

        restBool = true;
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
        // unsubscribe the events so it doesn't call an error while inactive
        Unsubscribe();
        // clear the variables used for sleeping the player object
        ClearSleepCheck();
        turnInitiated = false;
    }
}
