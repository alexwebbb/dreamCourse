using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {

    public Canvas canvas;
    public GameObject mainPanel;
    public GameObject levelSelect;

    SessionController sessionController;
    AssetManager assetManager;

	void Start () {

        // load session controller, asset manager
        sessionController = GetComponent<SessionController>();
        assetManager = GetComponent<AssetManager>();

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

    public void LevelLoadTest() {
        sessionController.StartGame(assetManager.level[0].GetComponent<Level>().fileName);
    }
}
