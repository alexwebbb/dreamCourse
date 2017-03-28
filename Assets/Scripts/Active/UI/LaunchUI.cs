using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LaunchUI : MonoBehaviour {

    // Launch UI handles player input related to the launch sequence

    public Slider forceSlider;
    public Slider medialSpinSlider;
    public Slider lateralSpinSlider;

    // events are used to communicate with gameobjects in scene

    public event Action launchToggleEvent;
    public event Action initiateLaunchEvent;
    public event Action<float> setForceEvent;
    public event Action<float> setMedialSpinEvent;
    public event Action<float> setLateralSpinEvent;
    // default launch values
    float defaultForce;
    float defaultMedialSpin;
    float defaultLateralSpin;
    // lerp components
    public float lerpSpeed = 1f;
    float lerpTime;
    float lerpMinimum;
    float lerpMaximum;
    bool launchModeActive;

    List<LaunchController> activeLaunchers = new List<LaunchController>();

    public void RegisterLauncher(LaunchController launcher) {
        activeLaunchers.Add(launcher);
        launcher.endTurnEvent += ResetLaunchValues;
    }

    
    void Start() {
        defaultForce = forceSlider.value;
        defaultMedialSpin = medialSpinSlider.value;
        defaultLateralSpin = lateralSpinSlider.value;
        lerpMinimum = forceSlider.minValue;
        lerpMaximum = forceSlider.maxValue;

        forceSlider.onValueChanged.AddListener((value) => SetForce(value));
        medialSpinSlider.onValueChanged.AddListener((value) => SetMedialSpin(value));
        lateralSpinSlider.onValueChanged.AddListener((value) => SetLateralSpin(value));
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            // this event is for toggling launch mode
            if (launchToggleEvent != null) {
                launchToggleEvent();
            }

            if (!launchModeActive) StartLerp();
            else StopLerp();
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            // sets the next gameobject to be launched as the player object
            if (initiateLaunchEvent != null) {
                initiateLaunchEvent();
            }

            StopLerp();
        }
    }

    IEnumerator LerpForce () {

        while (launchModeActive) {
            // set the value of the force slider
            forceSlider.value = Mathf.Lerp(lerpMinimum, lerpMaximum, lerpTime);

            // .. and increment the interpolater
            lerpTime += lerpSpeed * Time.deltaTime;
            Debug.Log("Time: " + lerpTime);
            Debug.Log("Min: " + lerpMinimum);
            Debug.Log("Max: " + lerpMaximum);

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (lerpTime > 1.0f) {
                float temp = lerpMaximum;
                lerpMaximum = lerpMinimum;
                lerpMinimum = temp;
                lerpTime = 0.0f;
            }

            yield return new WaitForEndOfFrame();

        }

        yield return null;

    }

    // these functions trigger events that act upon the launch controller
    void SetForce(float force) {
        // this is now used to set the secret playerForce
        if (setForceEvent != null) setForceEvent(force);
    }

    void SetMedialSpin(float medialSpin) {
        if (setMedialSpinEvent != null) setMedialSpinEvent(medialSpin);
    }

    void SetLateralSpin(float lateralSpin) {
        if (setLateralSpinEvent != null) setLateralSpinEvent(lateralSpin);
    }

    void ResetLaunchValues() {
        forceSlider.value = defaultForce;
        medialSpinSlider.value = defaultMedialSpin;
        lateralSpinSlider.value = defaultLateralSpin;
        lerpMinimum = forceSlider.minValue;
        lerpMaximum = forceSlider.maxValue;
    }

    void StartLerp() {
        launchModeActive = true;
        StartCoroutine("LerpForce");
    }

    void StopLerp() {
        launchModeActive = false;
        StopCoroutine("LerpForce");
    }

}
