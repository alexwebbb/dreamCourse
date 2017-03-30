using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public event Action<Character> setActivePlayerEvent;
    public event Action turnIsOverEvent;

    Level currentLevel;

    int numberOfPlayers;
    int scoreablePoints;

    // first field is player one, second is player 2
    List<Character> player = new List<Character>();
    Dictionary<Character, List<Goal>> score = new Dictionary<Character, List<Goal>>();
    
    int activePlayer;
    int turnNumber;

    public int GetTotalScored {
        get {
            int totalScored = 0;
            for (int i = 0; i < numberOfPlayers; i++) { totalScored += score[player[i]].Count; }
            return totalScored;
        }
    }

    public void RegisterGoal(Goal goal) {
        scoreablePoints += 1;

        goal.pointScoredEvent += AddPoint;
    }

    public void RegisterBoundingBox(BoundingBox boundingBox) {
        boundingBox.characterOutOfBoundsEvent += EndTurn;
    }

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

            player.Add(thisCharacter);

            // create score list for each character
            score.Add(thisCharacter, new List<Goal>());

            // initialize last position for each character
            thisCharacter.LastPosition = currentLevel.transform.position;

            // subscribe to beginning and end of turn events
            player[i].GetLaunchController.ballRestingEvent += EndTurn;
            player[i].GetLaunchController.beginTurnEvent += BeginTurn;

            if (i != 0) {
                player[i].SetHidden(true);
            }
        }

        activePlayer = 0;
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[0]);
        // reset turn number
        turnNumber = 0;
    }


    void EndTurn() {

        if (turnIsOverEvent != null) turnIsOverEvent();

        // potential camera switch check here

        if(!player[activePlayer].IsDead) player[activePlayer].SleepCharacterPosition();
        // else if(numberOfPlayers != 1) player[activePlayer].SetHidden(true);

        // activate player whose turn it shall be
        bool lastElement = false;

        if (activePlayer + 1 < numberOfPlayers) {
            
            // set the soon to be active player as visible if it has been hidden
            if (turnNumber == 0 || player[activePlayer + 1].IsDead) {
                player[activePlayer + 1].SetHidden(false);
            } else {
                player[activePlayer + 1].SetAsActivePlayer(true);
            }

        } else {
            // if it is the last element, jump to the beginning
            if (player[0].IsDead) player[0].SetHidden(false);
            else player[0].SetAsActivePlayer(true);

            lastElement = true;
            turnNumber += 1;
        }

        // deactivate the player who called the turn end
        if (numberOfPlayers != 1) {
            player[activePlayer].SetAsActivePlayer(false);
        }
        // if it is the last element in the list, set the index to zero, otherwise iterate
        activePlayer = lastElement ? 0 : activePlayer + 1;

        // call to the rest of the system changing the current active player
        if (setActivePlayerEvent != null) setActivePlayerEvent(player[activePlayer]);
    }


    void BeginTurn(bool reset) {
        // call the position reset on the now active player when the launch controller reports that it is ready
        if(reset) player[activePlayer].SleepCharacterPosition();
    }


    void AddPoint(Character scorer, Goal goal) {
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

        List<KeyValuePair<Character, List<Goal>>> sortedScoreList = score.ToList();

        sortedScoreList.Sort((pair1, pair2) => pair1.Value.Count.CompareTo(pair2.Value.Count));

        foreach (KeyValuePair<Character, List<Goal>> player in sortedScoreList) {
            Debug.Log("player " + player.Key.characterName + ": " + player.Value.Count);
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
