using UnityEngine;
using System;
using System.Collections;

public class LaunchController : MonoBehaviour {

    
    public GameObject tracerObject;
    public GameObject playerObject;

    [Header("Bounce Controller Items")]
    public Vector3 spinForce;
    public float spinForceFactor = 0.05f;
    public float bouncePercent = 0.50f;

    [Header("Launch Components")]
    public float defaultForce;
    public float minForce;
    public float maxForce;

    public float lifetime;
    public float ballGap;

    [Header("Rolling Characteristics")]
    public float angleDrag;
    public float drag;
    public float rollLimit = 5f;
    public float velocitySleep = 5f;

    // for grabbing the event parent
    LevelController levelController;
    LaunchUI launchUI;

    Character character;
    Transform pointerCube;
    Rigidbody playerObjectRB;
    ConstantScale playerObjectCS;

    int restCounter = 0;
    int restLimit = 150;

    bool launchModeIsActive;
    bool playerLaunchButtonHasBeenPressed;
    bool readyToRest;
    bool launchInitiated;
    bool playerReadyForLaunch;

    float normalizedForce;
    float GetPlayerForce { get { return (normalizedForce * (maxForce - minForce)) + minForce; } }

    [Header("Experimental Section")]
    public Vector3 centerOfMass;

    void Awake() {
        // grab the players rigidbody for launching purposes
        playerObjectRB = playerObject.GetComponent<Rigidbody>();

        foreach (IBallComponent component in playerObject.GetComponents<IBallComponent>()) {
            component.Initialize(playerObjectRB, this);
        }

        // grab the constant scale component of the player
        playerObjectCS = playerObject.GetComponent<ConstantScale>();

        // grab the pointer cube that is used to direct the aim of the launcher. 
        // the launcher determines launch direction by using this cube and the player object
        pointerCube = transform.GetChild(0).GetChild(0).GetChild(0);

        // get the launch UI to subscribe to it
        launchUI = FindObjectOfType<LaunchUI>();

        // register the launch controller with the launch UI for launch events, such as resetting values
        // will probably remove this
        launchUI.RegisterLauncher(this);
        // fetch the level controller
        levelController = FindObjectOfType<LevelController>();
        character = GetComponentInParent<Character>();
    }


    void OnTurnEnd() {
        // this covers both later and medial spin
        spinForce = Vector3.zero;

        // clear the variables used for sleeping the player object
        ClearRestCheck();

        // reset the variable that tracks the launch event 
        launchInitiated = false;
    }

    void FixedUpdate() {
        // this block is here in case the player hasn't moved at all.. so that it can skip that few second count down
        if(!playerReadyForLaunch && !launchInitiated && playerObjectRB.transform.position == transform.position) {
            // immediately turn off the variable that passed this block, so that begin turn is called only once
            playerReadyForLaunch = true;
            // set rest bool to false... needs to happen before begin turn is called
            ClearRestCheck();
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
                ClearRestCheck();
                
                if (launchInitiated) {
                    // call the end turn event in the levelcontroller.. most player switching logic is there
                    levelController.EndTurn();
                } else {

                    // call this at the beginning of the turn in case the player is rolling along as a result of other player action or hazards
                    levelController.BeginTurn(true);

                    // lock position at beginning of turn
                    playerObjectRB.constraints = RigidbodyConstraints.FreezeAll;

                }
            }
        }
    }


    void ClearRestCheck() {
        // these are the variables used in the fixed update to allow the launch controller to sleep the ball
        readyToRest = false;
        // rest counter is reset
        restCounter = 0;
    }

    void SetRestCheck(Character activeCharacter) {
        // to call when the player is starting their turn, so that begin turn can be called
        if(character == activeCharacter) readyToRest = true;
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

        GameObject launchingObject;

        if (traceBool) {
            // instantiate the tracer object
            launchingObject = (GameObject)Instantiate(tracerObject, playerObject.transform.position, playerObject.transform.localRotation);

            // set the launcher field in the bounce controller. BC uses the launcher to set initial launch qualities

            // copying the characteristics of the scaler to the instance object
            launchingObject.GetComponent<ConstantScale>().CopyScale(playerObjectCS);

            // grab the rb
            Rigidbody launchingRB = launchingObject.GetComponent<Rigidbody>();

            foreach(IBallComponent component in launchingObject.GetComponents<IBallComponent>()) {
                component.Initialize(launchingRB, this);
            }
            
            // look at the aiming cube
            launchingRB.transform.LookAt(pointerCube);
            
            // launch forces are applied
            launchingRB.AddRelativeForce(Vector3.forward * defaultForce, ForceMode.VelocityChange);
            launchingRB.AddRelativeTorque(spinForce, ForceMode.VelocityChange);

            // sets the decay time for the tracer objects
            Destroy(launchingObject, lifetime);

        } else {
            // point the player rb at the cube
            playerObjectRB.transform.LookAt(pointerCube);

            // launch forces are applied
            playerObjectRB.AddRelativeForce(Vector3.forward * GetPlayerForce, ForceMode.VelocityChange);
            playerObjectRB.AddRelativeTorque(spinForce, ForceMode.VelocityChange);
        }
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
        // subscribe to end of turn event for resetting values
        levelController.turnIsOverEvent += OnTurnEnd;
        // subscribe to set active player event, kind of like "awake" for the turn
        levelController.setActivePlayerEvent += SetRestCheck;
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
        // unsubscribe to end of turn event for resetting values
        levelController.turnIsOverEvent -= OnTurnEnd;
        // unsubscribe to set active player event, kind of like "awake" for the turn
        levelController.setActivePlayerEvent -= SetRestCheck;
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
        // unsubscribe the events so it doesn't call an error while inactive
        Unsubscribe();


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


