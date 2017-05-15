using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour {

    // property that keeps track of which step of the call list the new game is at
    // int callListCount;

    // list of userinterface components ... call list ... handled by properties
    // List<GameObject> callList = new List<GameObject>();

    // game mode will be provided with the call list

    bool callListSet = false;
    public bool IsCallListSet { get { return callListSet; } }

    List<GameObject> callList = new List<GameObject>();
    int current;

    public List<GameObject> SetCallList {
        set {
            callList = value;
            callListSet = true;
            current = 0;
        }
    }

    public ISelectionMenu GetCurrentMenu {
        get { return callList[current].GetComponent<ISelectionMenu>(); }
    }

    public GameObject NextMenu() {

        if(current < callList.Count) current++;
        return callList[current];

    }

    public GameObject PreviousMenu() {

        if(current > 0) current--;
        return callList[current];

    }

    // [Header("Don't leave this stuff public")]

    // character slection stuff

    int numberOfPlayers = 0;

    public int GetNumberOfPlayers { get { return numberOfPlayers; } }
    public int SetNumberOfPlayers { set { numberOfPlayers = value; } }

    List<Character> characterSelection = new List<Character>();

    public List<Character> GetCharacterSelection { get { return characterSelection; } }

    // level select stuff

    int numberOfLevels = 1; // like golf, maybe there are like prefab loadouts for cups and courses

    public int GetNumberOfLevels { get { return numberOfLevels; } }
    public int SetNumberOfLevels { set { numberOfLevels = value; } }

    public List<Level> levelSelection = new List<Level>();

    public List<Level> GetLevelSelection { get { return levelSelection; } }

    // confirmation prompt. function that checks if the necessary values are null or not.

    // properties also for returning all the above mentioned properties
    

}
