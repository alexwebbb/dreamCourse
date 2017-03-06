using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public event Action<GameObject> setActivePlayerEvent;

    AssetManager assetManager;
    GameObject levelOrigin;

    Vector3 startPosition;
    int numberOfPlayers;

    GameObject[] player = new GameObject[2];
    List<Vector3> playerPositions;

    float time;

    int playerTurn;

    // or maybe turnNumber
    int jumpCount = 0;

    // first field is player one, second is player 2
    int[] score;


    void Start() {

        assetManager = GetComponent<AssetManager>();

    }


    // there should be a game mode enum here as well
    public void Initialize(GameObject[] character) {

        Debug.Log("Initialized");

        levelOrigin = GameObject.FindGameObjectWithTag("LevelOrigin");

        
        player[0] = Instantiate<GameObject>(character[0], levelOrigin.transform, false);

        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);

        // this gets called from session manager
        // might actually have to do something with the SceneManager.sceneLoaded event to trigger this

        // reset all lists and variables to zero

        // load save data if there is any
        // set start position.....hmm maybe actually not. I should just create a start position gameobject with a tag
        // set number of players
        // instantiate player objects at start position
        // deactivate all except player 1
        // tie camera controller to player camera transform


    }


    void EndTurn() {

        // end the current players turn. called from launch controller or player controller once reset position is called. returns landing position and adds it to the player positions list
    }

    void AddPoint() {
        // add a point to the score. called from goal gameobjects
    }

    void ExportLevelSession() {
        // used to export an in progress game to save file, or a record of a completed match (like when someone completes a level in single player mode
    }


}
