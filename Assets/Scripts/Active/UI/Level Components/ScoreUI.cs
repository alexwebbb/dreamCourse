using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : LevelComponent {

    public GameObject scoreListIcon;
    public Text scoreHeader;
    public Transform scoreList;
    
	// Update is called once per frame
	void Start () {
        GetLevelController.levelIsLoaded += InitializePlayerList;
        GetLevelController.scoreAdjusted += CheckScore;
	}



    void InitializePlayerList() {
        foreach (Character player in GetLevelController.GetPlayers) {
            if (player != null) {
                
                GameObject playerButton = Instantiate<GameObject>(scoreListIcon);
                playerButton.transform.SetParent(scoreList, false);
                playerButton.GetComponentInChildren<Text>().text = player.characterName + ": 0";
            }
        }
    }

    void CheckScore() {


        // just a fun little check of the score
        int totalScored = GetLevelController.GetTotalScored;
        Debug.Log("Scoreable Points: " + GetLevelController.ScoreablePoints);
        Debug.Log("Total Points Scored: " + totalScored);


        List<KeyValuePair<Character, int>> sortedScoreList = new List<KeyValuePair<Character, int>>();

        foreach (KeyValuePair<Character, List<Goal>> player in GetLevelController.GetScore) {

            sortedScoreList.Add(new KeyValuePair<Character, int>(player.Key, player.Value.Count));
        }

        sortedScoreList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        int i = 0;
        foreach (KeyValuePair<Character, int> player in sortedScoreList) {
            string message = player.Key.characterName + ": " + player.Value;
            Debug.Log(message);
            scoreList.GetChild(i).GetComponentInChildren<Text>().text = message;
            i++;
        }

        if (totalScored == GetLevelController.ScoreablePoints) {
            string message = sortedScoreList[0].Key.characterName + " is ahead! The exit is now open!";
            Debug.Log(message);
            scoreHeader.GetComponentInChildren<Text>().text = message;
        }

    }
}
