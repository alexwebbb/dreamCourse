﻿using UnityEngine;
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
        launchUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        sessionController.StartGame(filename, assetManager.character);
        

    }

    void InitializeLevelList() {
        foreach(GameObject levelGameObject in assetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(levelList.transform, false);
                levelButton.GetComponentInChildren<Text>().text = level.levelName;

                levelButton.GetComponent<Button>().onClick.AddListener(() => LevelLoadTest(level.fileName)); 
            }
        }
    }
}
