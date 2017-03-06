using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {

    public GameObject levelListButton;
    [Space(10)]
    public GameObject canvas;
    public GameObject mainPanel;
    public GameObject levelSelect;
    public GameObject levelList;
    [Space(10)]
    public GameObject launchUI;

    SessionController sessionController;
    AssetManager assetManager;

	void Start () {

        // load session controller, asset manager
        sessionController = GetComponent<SessionController>();
        assetManager = GetComponent<AssetManager>();

        InitializeLevelList();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void NewGameButton() {
        Debug.Log("newgame clicked");

        // animate first layer to a little block in the corner
        // deactivate buttons

        mainPanel.SetActive(false);
        levelSelect.transform.GetChild(0).gameObject.SetActive(true);


        // set as not interactable
    }

    public void BackToMainMenu() {
        levelSelect.transform.GetChild(0).gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void LevelLoadTest(String filename) {
        canvas.SetActive(false);
        sessionController.StartGame(filename);
        // launchUI.SetActive(true);
        // the following will NOT be done here, but rather in the level controller initialize function in production

        Debug.Log(assetManager.character[0]);
        Debug.Log(assetManager.level[0].transform.position);

        GameObject poop = Instantiate<GameObject>(assetManager.character[0], assetManager.level[0].transform.position, Quaternion.identity);

        Debug.Log(poop);
        /*
        Debug.Log(character);
        CameraController camController = launchUI.GetComponentInChildren<CameraController>();
        camController.player = character.transform.GetChild(0).GetChild(0).gameObject;
        camController.cameraTransform = character.transform.GetChild(1);
        */

    }

    void InitializeLevelList() {
        foreach(GameObject levelGameObject in assetManager.level) {
            Level level = levelGameObject.GetComponent<Level>();

            GameObject levelButton = Instantiate<GameObject>(levelListButton, levelList.transform);
            levelButton.GetComponentInChildren<Text>().text = level.levelName;

            levelButton.GetComponent<Button>().onClick.AddListener(() => LevelLoadTest(level.fileName));
        }
    }
}
