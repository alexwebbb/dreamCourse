using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public GameObject GetPlayerContainer {
        get {
            if(playerContainer == null) playerContainer = transform.GetChild(0).gameObject;
            return playerContainer;
        }
    }

    public GameObject GetPlayer {
        get {
            if (player == null) player = player = transform.GetChild(0).GetChild(0).gameObject;
            return player;
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

    public GameObject playerContainer;
    public GameObject player;
    public GameObject playerLauncher;
    public LaunchController launchController;
    public Transform cameraTransform;
    public GameObject cameraContainer;

	public void SetAsActivePlayer(bool active) {

        GetCameraContainer.SetActive(active);
        GetPlayerLauncher.SetActive(active);

    }

    public void SetAsVisiblePlayer(bool visible) {

        GetPlayerContainer.SetActive(visible);

    }

    public void SetHidden(bool hiding) {

        if (hiding) {

            SetAsActivePlayer(false);
            SetAsVisiblePlayer(false);

        } else if (!hiding) {

            SetAsVisiblePlayer(true);
            SetAsActivePlayer(true);

        }
    }

}
