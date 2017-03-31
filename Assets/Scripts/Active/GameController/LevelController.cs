﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public event Action<Character> setActivePlayerEvent;
    public event Action turnIsOverEvent;
    public event Action<bool> playerIsReadyEvent;

    Level currentLevel;

    public int NumberOfPlayers { get; protected set; }
    int scoreablePoints;

    // first field is player one, second is player 2
    List<Character> player = new List<Character>();
    Dictionary<Character, List<Goal>> score = new Dictionary<Character, List<Goal>>();
    
    int activePlayer;
    int turnNumber;

    public int GetTotalScored {
        get {
            int totalScored = 0;
            for (int i = 0; i < NumberOfPlayers; i++) totalScored += score[player[i]].Count;
            return totalScored;
        }
    }

    public void RegisterGoal(Goal goal) {
        scoreablePoints += 1;
    }

    public void RegisterBoundingBox(BoundingBox boundingBox) {
        boundingBox.characterOutOfBoundsEvent += EndTurn;
    }
    
    public void Initialize(List<Character> character, int _numberOfPlayers) {
        // initialize number of players
        NumberOfPlayers = _numberOfPlayers;
        // set current level so that calls to custom level object can be made
        currentLevel = GameObject.FindGameObjectWithTag("LevelOrigin").GetComponent<Level>();
        // begin instantiating characters
        GameObject instantiatedCharacter;
        for (int i = 0; i < NumberOfPlayers; i++) {
            // it is necessary to instantiate the game object first since that is what monobehavior scripts require
            instantiatedCharacter = Instantiate<GameObject>(character[i].gameObject, currentLevel.transform, false);
            // grab the character component of the character gameobject we just instantiated
            Character thisCharacter = instantiatedCharacter.GetComponent<Character>();
            // add instantiated character to local player list. player order is maintained there
            player.Add(thisCharacter);
            // create score list for each character
            score.Add(thisCharacter, new List<Goal>());
            // initialize last position for each character
            thisCharacter.LastPosition = currentLevel.transform.position;
            // hide the characters except for the first one for the first round
            if (i != 0) { player[i].SetHidden(true); }
        }
        // set the active player to 0 since that it is first turn 
        activePlayer = 0;
        // call event that attaches the active player to the camera
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);
        // reset turn number
        turnNumber = 0;
    }


    public void EndTurn() {
        // event signaling that the turn is over, reset methods can be called for UI elements
        if (turnIsOverEvent != null) turnIsOverEvent();
        // potential camera switch check here

        // sleep the character after returning it, as long as it isnt dead
        if(!player[activePlayer].IsDead) player[activePlayer].SleepCharacterPosition();
        // if the player is dead, hide it.
        else if(NumberOfPlayers != 1) player[activePlayer].SetHidden(true);
        // begin sequence to activate player whose turn it shall be
        bool lastElement = false;
        if (activePlayer + 1 < NumberOfPlayers) {
            // set the soon to be active player as visible if it has been hidden
            if (turnNumber == 0 || player[activePlayer + 1].IsDead) {
                // player may be on initial turn or dead. set her active!
                player[activePlayer + 1].SetHidden(false);
            } else {
                // set her active also if she is not hidden
                player[activePlayer + 1].SetAsActivePlayer(true);
            }
        } else {
            // if its in this block, that means it is the last player
            lastElement = true;
            // since it is the last player, we increment the turn
            turnNumber += 1;
            // unhide that player 1!
            if (player[0].IsDead) player[0].SetHidden(false);
            // or just him active since he was well behaved
            else player[0].SetAsActivePlayer(true);
        }
        // deactivate the player who called the turn end
        if (NumberOfPlayers != 1) player[activePlayer].SetAsActivePlayer(false);
        // if it is the last player to go, set the index to zero, otherwise iterate
        activePlayer = lastElement ? 0 : activePlayer + 1;
        // call to the rest of the system changing the current active player
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[activePlayer]);
    }


    public void BeginTurn(bool reset) {
        // call the position reset on the now active player when the launch controller reports that it is ready
        if(reset) player[activePlayer].SleepCharacterPosition();
        if (playerIsReadyEvent != null) playerIsReadyEvent(true);
    }


    public void AddPoint(Character scorer, Goal goal) {
        // add a point to the score. called from goal gameobjects
        if(!score[scorer].Contains(goal)) score[scorer].Add(goal);
        // change the color of the goal object
        goal.SetColor = scorer.captureColor;
        // if the goal is changing hands, remove the point from the previous owner
        if (goal.GetLastOwner != null && scorer != goal.GetLastOwner) { score[goal.GetLastOwner].Remove(goal); }
        CheckScore();
    }

    void CheckScore() {
        // just a fun little check of the score
        int totalScored = GetTotalScored;
        Debug.Log("Scoreable Points: " + scoreablePoints);
        Debug.Log("Total Points Scored: " + totalScored);


        List<KeyValuePair<Character, int>> sortedScoreList = new List<KeyValuePair<Character, int>>();

        foreach(KeyValuePair<Character, List<Goal>> player in score) {
            
            sortedScoreList.Add(new KeyValuePair<Character, int>(player.Key, player.Value.Count));
        }

        sortedScoreList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        
        foreach (KeyValuePair<Character, int> player in sortedScoreList) {
            Debug.Log("player " + player.Key.characterName + ": " + player.Value);
        }
        
        if(totalScored == scoreablePoints) {
            Debug.Log(sortedScoreList[0].Key.characterName + " wins!");
        }

    }

    
    /* future plans
     * 
    void ExportLevelSession() {
        // used to export an in progress game to save file, or a record of a completed match (like when someone completes a level in single player mode
    }
    */

}
