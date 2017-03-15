using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    public SessionController GetSessionController {
        get {
            if (sessionController == null) sessionController = transform.parent.GetComponent<SessionController>();
            return sessionController;
        }
    }

    public GameObject GetMainMenu {
        get {
            if (mainMenu == null) mainMenu = transform.FindChild("Main Menu").gameObject;
            return mainMenu;
        }
    }

    public GameObject GetLaunchUI {
        get {
            if (launchUI == null) launchUI = transform.FindChild("Launch UI").gameObject;
            return launchUI;
        }
    }

    SessionController sessionController;
    GameObject mainMenu;
    GameObject launchUI;




    public void LoadLevel(NewGame newGame) {

        GetSessionController.StartGame(newGame);
        GetMainMenu.SetActive(false);
        GetLaunchUI.SetActive(true);
    }

}
