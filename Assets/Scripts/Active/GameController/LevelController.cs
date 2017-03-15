﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public event Action<Character> setActivePlayerEvent;

    AssetManager assetManager;
    GameObject levelOrigin;

    Vector3 startPosition;
    int numberOfPlayers;

    // first field is player one, second is player 2
    List<Character> player = new List<Character>();
    List<Vector3> playerPositions = new List<Vector3>();

    float time;

    int activePlayer;

    bool firstTurn;
    
    int jumpCount = 0;

    
    int[] score;


    void Start() {

        assetManager = GetComponent<AssetManager>();

    }

    // there should be a game mode enum here as well
    public void Initialize(List<Character> character, int _numberOfPlayers) {

        Debug.Log("Initialized");

        numberOfPlayers = _numberOfPlayers;

        levelOrigin = GameObject.FindGameObjectWithTag("LevelOrigin");

        // begin instantiating characters
        GameObject instantiatedCharacter;

        for (int i = 0; i < numberOfPlayers; i++) {

            // it is necessary to instantiate the game object first since that is what monobehavior scripts require
            instantiatedCharacter = Instantiate<GameObject>(character[i].gameObject, levelOrigin.transform, false);

            // don't worry, gameobject can still be called by using gameObject
            player.Add(instantiatedCharacter.GetComponent<Character>());
            
            // subscribe to end launch event
            player[i].GetLaunchController.endLaunchEvent += EndTurn;

            if (i != 0) {
                player[i].SetHidden(true);
            }
        }

        // this might could be a seperate method, set active player
        activePlayer = 0;
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);

        firstTurn = true;


        // this gets called from session manager
        
        // reset all lists and variables to zero

        // load save data if there is any
    }


    void EndTurn() {

        // this first part could eventually be exported to something like transfer camera. would want to rename active player to like active thing and have a separate current player variable.

        // activate player whose turn it shall be
        bool lastElement = false;

        if (activePlayer + 1 < numberOfPlayers) {

            Debug.Log("active player " + activePlayer);
            Debug.Log("number of players " + numberOfPlayers);

            // set the soon to be active player as visible if it has been hidden
            if (firstTurn) {
                player[activePlayer + 1].SetHidden(false);
            } else {
                player[activePlayer + 1].SetAsActivePlayer(true);
            }

        } else {
            // if it is the last element, jump to the beginning
            player[0].SetAsActivePlayer(true);
            lastElement = true;
            firstTurn = false;
        }

        Debug.Log("last element " + lastElement);

        // deactivate the player who called the turn end
        player[activePlayer].SetAsActivePlayer(false);
        
        // if it is the last element in the list, set the index to zero, otherwise iterate
        activePlayer = lastElement ? 0 : activePlayer + 1;

        
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
