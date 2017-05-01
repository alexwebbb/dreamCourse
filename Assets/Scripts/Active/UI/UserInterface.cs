using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    SessionController sessionController;
    GameObject mainMenu;
    GameObject launchUI;
    GameObject scoreUI;

    void Start() {
        sessionController = transform.parent.GetComponent<SessionController>();
        mainMenu = transform.Find("Main Menu").gameObject;
        launchUI = transform.Find("Level UI").gameObject;
        scoreUI = transform.Find("Score UI").gameObject;
    }


    public void LoadLevel(NewGame newGame) {

        sessionController.StartGame(newGame);
        mainMenu.SetActive(false);
        launchUI.SetActive(true);
        scoreUI.SetActive(true);

    }

}
