using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour {

    // property that keeps track of which step of the call list the new game is at
    // int callListCount;

    // list of userinterface components ... call list ... handled by properties
    // List<ISelectionMenu> callList = new List<ISelectionMenu>();

    // game mode will be provided with the call list

    Stack<ISelectionMenu> callListPre = new Stack<ISelectionMenu>();
    Stack<ISelectionMenu> callListPost = new Stack<ISelectionMenu>();

    public Stack<ISelectionMenu> SetCallList { set { callListPre = value; } }

    public ISelectionMenu NextMenu() {

        callListPost.Push(callListPre.Peek());
        return callListPre.Pop();

    }

    public ISelectionMenu PreviousMenu() {

        callListPre.Push(callListPost.Peek());
        return callListPost.Pop();

    }


    // character slection stuff

    int numberOfPlayers;

    public int GetNumberOfPlayers { get { return numberOfPlayers; } }
    public int SetNumberOfPlayers { set { numberOfPlayers = value; } }    

    List<Character> characterSelection = new List<Character>();

    public List<Character> GetCharacterSelection { get { return characterSelection; } }

    // level select stuff

    int numberOfLevels; // like golf, maybe there are like prefab loadouts for cups and courses

    public int GetNumberOfLevels { get { return numberOfLevels; } }
    public int SetNumberOfLevels { set { numberOfLevels = value; } }

    List<Level> levelSelection = new List<Level>();

    public List<Level> GetLevelSelection { get { return levelSelection; } }

    // confirmation prompt. function that checks if the necessary values are null or not.

    // properties also for returning all the above mentioned properties
    

}
