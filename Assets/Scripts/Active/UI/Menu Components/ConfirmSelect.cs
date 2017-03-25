using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelect : MenuComponent, ISelectionMenu {

    // supposed to be private
    public NewGame newGame;

    public override void Start() {
        base.Start();
    }

    // Called when loading and unloading the menu, required by the interface

    public void Initialize(NewGame _newGame) {
        // set reference to new game object
        newGame = _newGame;
    }

    public void ResetMenu() {
        
    }

    public void ConfirmSelection() {
        mainMenu.Ready(newGame);
    }

}
