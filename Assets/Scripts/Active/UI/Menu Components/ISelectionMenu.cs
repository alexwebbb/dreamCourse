using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionMenu {

    MainMenu GetMainMenu {
        get;
    }

    GameObject GetElements {
        get;
    }

    // implement back function, which sets the iteration for call list count back one, sets the appropriate values to null, unsets whatever visual changes were made in initialize (calls unload) and then from there calls the initialize or next function for the prior menu

    // implement next function, which iterates the call list count by one, checks to see whether this next menu should be skipped or not based on current status of newgame object, and then calls the initialize function on the next menu

    // if call list is at the end, the next will call confirmation prompt. a game mode selection screen is what determines the call order. if players make decisions which require extra menu choices, it just adds them to the end.

    // initialize function, sets current UI status (should be an idempotent action that sets the active status of all game objects involved in the scene. might be an external generic command, where the current gameobject is submitted) might rename this to load / unload. Also loads all internal UI elements (call generic button generate function)

    void Initialize(NewGame newGame);
    // assigns the newgame property to a local variable

    void Next(NewGame newgame);
    // calls the Load function from main menu on the next menu function

    void Back(NewGame newgame);
    // call the Load Function from main menu on the back function. if necessary, clears data
}
