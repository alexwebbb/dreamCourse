using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public GameObject GetPlayer { get { return player; } }
    public Transform GetCameraTransform { get { return cameraTransform; } }
    public LaunchController GetLaunchController { get { return launchController; } }

    GameObject playerContainer;
    GameObject player;
    GameObject playerLauncher;
    LaunchController launchController;
    Transform cameraTransform;
    GameObject cameraContainer;

    void Start() {

        // be sure to change this if the filestructure for the character changes.... I would use a loop to find it, but this is so much lighter... SORRY
        playerContainer = transform.GetChild(0).gameObject;
        player = playerContainer.transform.GetChild(0).gameObject;
        playerLauncher = playerContainer.transform.GetChild(1).gameObject;
        launchController = playerLauncher.GetComponent<LaunchController>();
        cameraTransform = transform.GetChild(1);
        cameraContainer = cameraTransform.gameObject;
    }

	public void SetAsActivePlayer(bool active) {

        cameraContainer.SetActive(active);
        playerLauncher.SetActive(active);

    }

    public void SetAsVisiblePlayer(bool visible) {

        playerContainer.SetActive(visible);

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
