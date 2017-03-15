using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelect : MonoBehaviour, ISelectionMenu {

    MainMenu mainMenu;
    GameObject elements;

    // supposed to be private
    public NewGame newGame;

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.FindChild("Elements").gameObject;
            return elements;
        }
    }

    void Start() {
        mainMenu = transform.GetComponentInParent<MainMenu>();
    }

    // Called when loading and unloading the menu, required by the interface

    public void Initialize(NewGame _newGame) {
        // set reference to new game object
        newGame = _newGame;
    }

    public void ResetMenu() {
        
    }

    public void Confirmed() {
        mainMenu.Ready(newGame);
    }

}
