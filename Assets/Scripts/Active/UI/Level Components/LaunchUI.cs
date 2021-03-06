﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LaunchUI : MonoBehaviour {

    LevelController lc;

    // Launch UI handles player input related to the launch sequence

    public Slider forceSlider;
    public Slider medialSpinSlider;
    public Slider lateralSpinSlider;

    // events are used to communicate with gameobjects in scene

    public event Action launchModeToggleEvent;
    public event Action initiateLaunchEvent;
    public event Action<float> setForceEvent;
    public event Action<float> setMedialSpinEvent;
    public event Action<float> setLateralSpinEvent;

    // lerp components
    float lerpSpeed = 1f;
    bool launchModeActive;
    bool playerReady;

    List<LaunchController> activeLaunchers = new List<LaunchController>();

    public void RegisterLauncher(LaunchController launcher) {
        // probably going to remove this, we'll see
        activeLaunchers.Add(launcher);
    }

    
    void Start() {
        lc = FindObjectOfType<LevelController>();
        lc.turnIsOverEvent += ResetLaunchValues;
        lc.playerIsReadyEvent += ((value) => playerReady = value);

        forceSlider.onValueChanged.AddListener((value) => SetForce(value));
        medialSpinSlider.onValueChanged.AddListener((value) => SetMedialSpin(value));
        lateralSpinSlider.onValueChanged.AddListener((value) => SetLateralSpin(value));
    }

    void Update() {

        if (playerReady) {
            if (Input.GetKeyDown(KeyCode.L)) {
                ToggleLerp(!launchModeActive);
                // this event is for toggling launch mode
                if (launchModeToggleEvent != null) launchModeToggleEvent();
            }

            if (launchModeActive && Input.GetKeyDown(KeyCode.Space)) {
                playerReady = false;
                ToggleLerp(false);
                // sets the next gameobject to be launched as the player object
                if (initiateLaunchEvent != null) { initiateLaunchEvent(); }
            } 
        }
    }

    IEnumerator LerpForce () {

        float lerpTime = 0f;
        float lerpMinimum = 0f;
        float lerpMaximum = 1f;

        while (launchModeActive) {
            // set the value of the force slider
            forceSlider.normalizedValue = Mathf.Lerp(lerpMinimum, lerpMaximum, lerpTime);

            // .. and increment the interpolater
            lerpTime += lerpSpeed * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (lerpTime > 1.0f) {
                float temp = lerpMaximum;
                lerpMaximum = lerpMinimum;
                lerpMinimum = temp;
                lerpTime = 0.0f;
            }

            if(!launchModeActive) yield return null;

            yield return new WaitForEndOfFrame();

        }

        yield return null;

    }

    // these functions trigger events that act upon the launch controller
    void SetForce(float normalizedForce) {
        // this is now used to set the secret playerForce
        if (setForceEvent != null) setForceEvent(normalizedForce);
    }

    void SetMedialSpin(float medialSpin) {
        if (setMedialSpinEvent != null) setMedialSpinEvent(medialSpin);
    }

    void SetLateralSpin(float lateralSpin) {
        if (setLateralSpinEvent != null) setLateralSpinEvent(lateralSpin);
    }

    void ResetLaunchValues() {
        forceSlider.normalizedValue = 0.5f;
        medialSpinSlider.value = 0;
        lateralSpinSlider.value = 0;
    }

    public void ToggleLerp(bool on) {
        if (on) {
            launchModeActive = true;
            StartCoroutine("LerpForce");
        } else {
            launchModeActive = false;
            StopCoroutine("LerpForce");
        }
    }
}
