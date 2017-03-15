﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    SessionController sessionController;
    GameObject mainMenu;
    GameObject launchUI;

    void Start() {
        sessionController = transform.parent.GetComponent<SessionController>();
        mainMenu = transform.FindChild("Main Menu").gameObject;
        launchUI = transform.FindChild("Launch UI").gameObject;
    }


    public void LoadLevel(NewGame newGame) {

        sessionController.StartGame(newGame);
        mainMenu.SetActive(false);
        launchUI.SetActive(true);
    }

}
