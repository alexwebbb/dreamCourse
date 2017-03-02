using UnityEngine;
using System;
using System.Collections;

public class LaunchUI : MonoBehaviour {

    // Launch UI handles player input related to the launch sequence

    // events are used to communicate with gameobjects in scene

    public event Action launchToggleEvent;
    public event Action initiateLaunchEvent;
    public event Action<float> setForceEvent;
    public event Action<float> setMedialSpinEvent;
    public event Action<float> setLateralSpinEvent;


    void Update() {

        // YES I need to start unsubscribing event delegates

        if (Input.GetKeyDown(KeyCode.L)) {
            // this event is for toggling launch mode
            if (launchToggleEvent != null) {
                launchToggleEvent();
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            // sets the next gameobject to be launched as the player object
            if (initiateLaunchEvent != null) {
                initiateLaunchEvent();
            }
        }
    }

    // these functions trigger events that act upon the launch controller
    public void SetForce(float force) {
        if (setForceEvent != null) setForceEvent(force);
    }

    public void SetMedialSpin(float medialSpin) {
        if (setMedialSpinEvent != null) setMedialSpinEvent(medialSpin);
    }

    public void SetLateralSpin(float lateralSpin) {
        if (setLateralSpinEvent != null) setLateralSpinEvent(lateralSpin);
    }
}
