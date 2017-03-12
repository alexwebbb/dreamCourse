using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {


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


    public void Unload() {

        // hell, doesn't even seem like I need this one now

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
}
