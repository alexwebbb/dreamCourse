using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public string characterName;
    public Color captureColor;

    // private variable initialization
    GameObject playerContainer;
    GameObject player;
    Rigidbody playerRB;
    BounceController playerBC;
    GameObject playerLauncher;
    LaunchController launchController;
    Transform cameraTransform;
    GameObject cameraContainer;

    // utility properties

    public bool IsDead { get; protected set; }
    public Vector3 LastPosition { get; set; }
 
    // property accessors
    public GameObject GetPlayerContainer {
        get {
            if(playerContainer == null) playerContainer = transform.GetChild(0).gameObject;
            return playerContainer;
        }
    }

    public GameObject GetPlayer {
        get {
            if (player == null) player = GetPlayerContainer.transform.GetChild(0).gameObject;
            return player;
        }
    }

    public Rigidbody GetPlayerRigidbody {
        get {
            if (playerRB == null) playerRB = GetPlayer.GetComponent<Rigidbody>();
            return playerRB;
        }
    }

    public BounceController GetPlayerBounceController {
        get {
            if (playerBC == null) playerBC = GetPlayer.GetComponent<BounceController>();
            return playerBC;
        }
    }

    public GameObject GetPlayerLauncher {
        get {
            if (playerLauncher == null) playerLauncher = GetPlayerContainer.transform.GetChild(1).gameObject;
            return playerLauncher;
        }
    }

    public LaunchController GetLaunchController {
        get {
            if(launchController == null) launchController = GetPlayerLauncher.GetComponent<LaunchController>();
            return launchController;
        }
    }

    public Transform GetCameraTransform {
        get {
            if(cameraTransform == null) cameraTransform = transform.GetChild(1);
            return cameraTransform;
        }
    }

    public GameObject GetCameraContainer {
        get {
            if(cameraContainer == null) cameraContainer = GetCameraTransform.gameObject;
            return cameraContainer;
        }
    }


    // utility methods
	public void SetAsActivePlayer(bool active) {

        GetCameraContainer.SetActive(active);
        GetPlayerLauncher.SetActive(active);

    }

    void SetAsVisiblePlayer(bool visible) {

        GetPlayerContainer.SetActive(visible);
    }

    public void SetHidden(bool goingToBeHidden) {

        if (goingToBeHidden) {

            SetAsActivePlayer(false);
            SetAsVisiblePlayer(false);

        } else if (!goingToBeHidden) {

            SetAsVisiblePlayer(true);
            SetAsActivePlayer(true);
            IsDead = false;
        }
    }

    public void ReturnOutOfBoundsCharacterToLastPosition() {

        // stops movement
        GetPlayerRigidbody.angularVelocity = GetPlayerRigidbody.velocity = Vector3.zero;

        // resets localRotation
        GetPlayer.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        // return character to last launch position
        GetPlayer.transform.position = LastPosition;

        IsDead = true;
    }

    public void SleepCharacterPosition() {

        // this resets the bounce counter that is attached to the player
        GetPlayerBounceController.bounceCount = 0;

        // stops movement
        GetPlayerRigidbody.angularVelocity = GetPlayerRigidbody.velocity = Vector3.zero;

        // resets localRotation
        GetPlayer.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        // pops the launcher over to the position of the player
        GetLaunchController.transform.position = GetPlayer.transform.position;

        // update the last position dictionary
        LastPosition = GetPlayer.transform.position;
    }

}
