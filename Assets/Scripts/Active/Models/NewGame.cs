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
    public bool IsCallList { get { return callListSet; } }

    Stack<GameObject> callListPre = new Stack<GameObject>();
    GameObject callListCurrent;
    Stack<GameObject> callListPost = new Stack<GameObject>();

    public Stack<GameObject> SetCallList {
        set {
            callListPre = value;
            callListSet = true;
            callListCurrent = callListPre.Pop();
        }
    }

    public ISelectionMenu GetCurrentMenu {
        get { return callListCurrent.GetComponent<ISelectionMenu>(); }
    }

    public GameObject NextMenu() {

        callListPost.Push(callListCurrent);
        callListCurrent = callListPre.Pop();
        return callListCurrent;

    }

    public GameObject PreviousMenu() {

        callListPre.Push(callListCurrent);
        callListCurrent = callListPost.Pop();
        return callListCurrent;

    }

    [Header("Don't leave this stuff public")]

    // character slection stuff

    public int numberOfPlayers;

    public int GetNumberOfPlayers { get { return numberOfPlayers; } }
    public int SetNumberOfPlayers { set { numberOfPlayers = value; } }

    List<Character> characterSelection = new List<Character>();

    public List<Character> GetCharacterSelection { get { return characterSelection; } }

    // level select stuff

    int numberOfLevels; // like golf, maybe there are like prefab loadouts for cups and courses

    public int GetNumberOfLevels { get { return numberOfLevels; } }
    public int SetNumberOfLevels { set { numberOfLevels = value; } }

    public List<Level> levelSelection = new List<Level>();

    public List<Level> GetLevelSelection { get { return levelSelection; } }

    public void ClearLevelSelection() {

        levelSelection.Clear();
    }

    // confirmation prompt. function that checks if the necessary values are null or not.

    // properties also for returning all the above mentioned properties
    

}
