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

    // first field is player one, second is player 2
    List<GameObject> player;
    List<Vector3> playerPositions;

    float time;

    int activePlayer;

    bool firstTurn;

    // or maybe turnNumber
    int jumpCount = 0;

    
    int[] score;


    void Start() {

        assetManager = GetComponent<AssetManager>();

    }


    // there should be a game mode enum here as well
    public void Initialize(GameObject[] character, int _numberOfPlayers) {

        Debug.Log("Initialized");

        numberOfPlayers = _numberOfPlayers;

        levelOrigin = GameObject.FindGameObjectWithTag("LevelOrigin");

        for (int i = 0; i < numberOfPlayers; i++) {

            player.Add(Instantiate<GameObject>(character[i], levelOrigin.transform, false));

            if(i != 0) {
                player[i].GetComponent<Character>().SetHidden(true);
            }
        }

        // this might could be a seperate method, set active player
        activePlayer = 0;
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);

        firstTurn = true;


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

        // this first part could eventually be exported to something like transfer camera. would want to rename active player to like active thing and have a separate current player variable.

        // activate player whose turn it shall be
        bool lastElement = false;

        if (activePlayer < numberOfPlayers) {

            // set the soon to be active player as visible if it has been hidden
            if (firstTurn) player[activePlayer].GetComponent<Character>().SetHidden(false);

            player[activePlayer + 1].GetComponent<Character>().SetAsActivePlayer(true);

        } else {
            // if it is the last element, jump to the beginning
            player[0].GetComponent<Character>().SetAsActivePlayer(true);
            lastElement = true;
        }

        // deactivate the player who called the turn end
        player[activePlayer].GetComponent<Character>().SetAsActivePlayer(false);
        
        // if it is the last element in the list, set the index to zero, otherwise iterate
        activePlayer = lastElement ? activePlayer + 1 : 0;

        
        // call to the rest of the system changing the current active player
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[activePlayer]);



        // end the current players turn. called from launch controller or player controller once reset position is called. returns landing position and adds it to the player positions list
    }

    void AddPoint() {
        // add a point to the score. called from goal gameobjects
    }

    void ExportLevelSession() {
        // used to export an in progress game to save file, or a record of a completed match (like when someone completes a level in single player mode
    }


}
