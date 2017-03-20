using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public event Action<Character> setActivePlayerEvent;

    Level currentLevel;

    int numberOfPlayers;
    int scoreablePoints;

    // first field is player one, second is player 2
    List<Character> player = new List<Character>();
    Dictionary<Character, List<Goal>> score = new Dictionary<Character, List<Goal>>();
    
    // this will most likely also need to be a dictionary of lists eventually
    Dictionary<Character, Vector3> playerLastPosition = new Dictionary<Character, Vector3>();
    
    int activePlayer;
    int turnNumber;

    public int GetTotalScored {
        get {
            int totalScored = 0;
            for (int i = 0; i < numberOfPlayers; i++) {
                totalScored += score[player[i]].Count;
            }
            return totalScored;
        }
    }

    public void RegisterGoal() { scoreablePoints += 1; }

    // there should be a game mode enum here as well
    public void Initialize(List<Character> character, int _numberOfPlayers) {
        
        numberOfPlayers = _numberOfPlayers;

        // set current level so that calls to custom level object can be made
        currentLevel = GameObject.FindGameObjectWithTag("LevelOrigin").GetComponent<Level>();

        // begin instantiating characters
        GameObject instantiatedCharacter;

        for (int i = 0; i < numberOfPlayers; i++) {

            // it is necessary to instantiate the game object first since that is what monobehavior scripts require
            instantiatedCharacter = Instantiate<GameObject>(character[i].gameObject, currentLevel.transform, false);

            Character thisCharacter = instantiatedCharacter.GetComponent<Character>();
            // don't worry, gameobject can still be called by using gameObject
            player.Add(thisCharacter);

            // create score list for each character
            score.Add(thisCharacter, new List<Goal>());

            // initialize last position for each character
            playerLastPosition.Add(thisCharacter, currentLevel.transform.position);

            // subscribe to end launch event
            player[i].GetLaunchController.endLaunchEvent += EndTurn;

            if (i != 0) {
                player[i].SetHidden(true);
            }
        }

        
        activePlayer = 0;
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);
        // reset turn number
        turnNumber = 0;


        // this gets called from session manager
        
        // reset all lists and variables to zero

        // load save data if there is any
    }


    void EndTurn() {

        // this first part could eventually be exported to something like transfer camera. would want to rename active player to like active thing and have a separate current player variable.
        playerLastPosition[player[activePlayer]] = player[activePlayer].GetPlayer.transform.position;

        // activate player whose turn it shall be
        bool lastElement = false;

        if (activePlayer + 1 < numberOfPlayers) {

            // set the soon to be active player as visible if it has been hidden
            if (turnNumber == 0) {
                player[activePlayer + 1].SetHidden(false);
            } else if(player[activePlayer + 1].IsDead) {
                player[activePlayer + 1].SetHidden(false);
                player[activePlayer + 1].IsDead = false;
            } else {
                player[activePlayer + 1].SetAsActivePlayer(true);
            }

        } else {
            // if it is the last element, jump to the beginning
            player[0].SetAsActivePlayer(true);
            lastElement = true;
            turnNumber += 1;
        }

        // deactivate the player who called the turn end
        if(numberOfPlayers != 1) player[activePlayer].SetAsActivePlayer(false);
        
        // if it is the last element in the list, set the index to zero, otherwise iterate
        activePlayer = lastElement ? 0 : activePlayer + 1;

        
        // call to the rest of the system changing the current active player
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[activePlayer]);
    }

    public void AddPoint(Character scorer, Goal goal) {
        // add a point to the score. called from goal gameobjects
        if(!score[scorer].Contains(goal)) score[scorer].Add(goal);
        // change the color of the goal object
        goal.SetColor = scorer.captureColor;
        // if the goal is changing hands, remove the point from the previous owner
        if (goal.GetLastOwner != null && scorer != goal.GetLastOwner) { score[goal.GetLastOwner].Remove(goal); }
        // just a fun little check of the score
        Debug.Log("Scoreable Points: " + scoreablePoints);
        Debug.Log("Total Points Scored: " + GetTotalScored);
        Debug.Log("player 1: " + score[player[0]].Count);
        Debug.Log("player 2: " + score[player[1]].Count);
    }

    public void ResetCharacterPosition(Character rc) {

        // rc stands for "returning character". u can't use player reset from launch controller for this, in case someone falls off the edge when it isn't their turn

        // stops movement
        rc.GetPlayerRigidbody.angularVelocity = rc.GetPlayerRigidbody.velocity = Vector3.zero;

        // resets localRotation
        rc.GetPlayer.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        
        // return character to last launch position
        rc.GetPlayer.transform.position = playerLastPosition[rc];

        // hide the character
        rc.SetHidden(true);

        // character is now dead! they fell off
        rc.IsDead = true;
    }

    void ExportLevelSession() {
        // used to export an in progress game to save file, or a record of a completed match (like when someone completes a level in single player mode
    }


}
