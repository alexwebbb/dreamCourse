using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // these two for testing
    public NewGame dummyNewGame;
    public List<GameObject> menuCallList;

    // not test

    UserInterface userInterface;
    List<GameObject> menuStack = new List<GameObject>();

    public void Next(NewGame _newgame) {

        GameObject nextMenu = _newgame.NextMenu();
        Load(nextMenu);
        nextMenu.GetComponent<ISelectionMenu>().Initialize(_newgame);

    }

    public void Back(NewGame _newgame) {

        _newgame.GetCurrentMenu.ResetMenu();
        GameObject prevMenu = _newgame.PreviousMenu();
        Load(prevMenu);
        prevMenu.GetComponent<ISelectionMenu>().Initialize(_newgame);

    }

    void Start() {
        userInterface = transform.parent.GetComponent<UserInterface>();

        PullMenuStack();
        Load(menuStack[0]);
    }

    void Load(GameObject thisMenu) {

        bool loadBool = false;

        foreach(GameObject menu in menuStack) {

            if (thisMenu == menu) {
                menu.SetActive(true);
                loadBool = true;
                continue;
            }

            menu.SetActive(loadBool);
            menu.GetComponent<ISelectionMenu>().GetElements.SetActive(false);
        }

        thisMenu.GetComponent<ISelectionMenu>().GetElements.SetActive(true);

    }

    void PullMenuStack() {

        foreach(Transform child in transform) {

            ISelectionMenu temp = child.GetComponent<ISelectionMenu>();
            
            if(temp != null) {
                menuStack.Add(child.gameObject);
            }
        }

        menuStack.Reverse();
    }
    

    // test section

    public void CreateDummyNewGame() {

        
        if(dummyNewGame.IsCallList == false) {
            List<GameObject> temp = menuCallList;
            temp.Reverse();
            dummyNewGame.SetCallList = new Stack<GameObject>(temp);
        }

        Next(dummyNewGame);
    }

    public void StepBackTest() {

        Back(dummyNewGame);

    }

    public void Ready(NewGame _newGame) {

        userInterface.LoadLevel(_newGame);

    }
}
