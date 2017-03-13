using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // these two for testing
    public NewGame dummyNewGame;
    public List<GameObject> menuCallList;

    // not test
    List<GameObject> menuStack = new List<GameObject>();

    void Start() {
        PullMenuStack();
        Load(menuStack[0]);
    }


    public void Load(GameObject thisMenu) {

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

        if(dummyNewGame.IsCallList == false) dummyNewGame.SetCallList = new Stack<GameObject>(menuCallList);
        Load(dummyNewGame.NextMenu());
    }

    public void StepBackTest() {

        Load(dummyNewGame.PreviousMenu());
    }
}
