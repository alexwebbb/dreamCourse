using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    Vector3 startPosition;
    int numberOfPlayers;

    GameObject[] player;
    List<Vector3> playerPositions;

    float time;

    int playerTurn;

    // or maybe turnNumber
    int jumpCount = 0;

    // first field is player one, second 
    int[] score;


	void Start () {
		
	}
	

	void Update () {
		
	}


    // there should be a game mode enum here as well
    void Initialize(int _numberOfPlayers, List<GameObject> selectedCharacters) {

        // this gets called from session manager

        // reset all lists and variables to zero

        // load save data if there is any
        // set start position.....hmm maybe actually not. I should just create a start position gameobject with a tag
        // set number of players
        // initialize player objects at start position
        // deactivate all except player 1


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
