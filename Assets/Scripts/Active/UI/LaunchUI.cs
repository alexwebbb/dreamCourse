using UnityEngine;
using System;
using System.Collections;

public class LaunchUI : MonoBehaviour {

    // here is where scripts will go that find and interface with the launch controller. player inputs will use scripts stored in this class

    public delegate void LaunchToggleDelegate();
    public event LaunchToggleDelegate launchToggleEvent;

    public delegate void InitiateLaunchDelegate();
    public event InitiateLaunchDelegate initiateLaunchEvent;

    void Update() {

        // need to find out if C# unloads references to destroyed gameobjects

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
}
