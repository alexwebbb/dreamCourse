using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    GameObject playerContainer;
    GameObject playerLauncher;
    GameObject cameraContainer;

    void Start() {

        // be sure to change this if the filestructure for the character changes.... I would use a loop to find it, but this is so much lighter... SORRY
        playerContainer = transform.GetChild(0).GetChild(0).gameObject;
        playerLauncher = transform.GetChild(0).GetChild(1).gameObject;
        cameraContainer = transform.GetChild(1).gameObject;

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
