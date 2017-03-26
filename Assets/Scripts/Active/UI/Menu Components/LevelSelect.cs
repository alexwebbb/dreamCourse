using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MenuComponent, ISelectionMenu {

    public GameObject levelListButton;
    public Color deselectedColor = Color.white;
    public Color selectedColor = Color.green;

    Transform levelList;
    //should be private
    public NewGame newGame;
    List<Button> levelButtons = new List<Button>();

    public override void Start() {
        base.Start();
        levelList = transform.Find("Elements/Level List Mask/Level List");

        // calls the initializer for the level buttons
        InitializeLevelList();
    }


    // Called when loading and unloading the menu, required by the interface

    public void Initialize(NewGame _newGame) {
        // set reference to new game object
        newGame = _newGame;
    }

    public void ResetMenu() {
        // clears level list on newgame
        ClearSelectedLevels();
    }


    void InitializeLevelList() {
        // all valid levels in the asset manager will be grabbed
        foreach (GameObject levelGameObject in assetManager.level) {
            // this is where we make sure there is a valid gameobject there, not an empty field
            if (levelGameObject != null) {
                // get the gameobject's level component
                Level level = levelGameObject.GetComponent<Level>();
                // if it doesn't have a level component, skip to the next object
                if (level == null) continue;
                // instantiate the button we have selected for this menu
                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                // place the button inside the list, gotta do it this way because of the world position boolean
                levelButton.transform.SetParent(levelList, false);
                // grab the button's text and change it to the name we have selected
                levelButton.GetComponentInChildren<Text>().text = level.levelName;
                // get the button component so we can add a listener to it
                Button levelButtonComponent = levelButton.GetComponent<Button>();
                // add listener for click event on buttons
                levelButtonComponent.onClick.AddListener(() => LevelSelected(level, levelButtonComponent));
                // add buttons to the local button list
                levelButtons.Add(levelButtonComponent);
            }
        }
    }

    void LevelSelected(Level _selectedLevel, Button button) {
        // fetch the level list
        List<Level> currentLevelSelection = newGame.GetLevelSelection;
        // is the level in there already?
        if (currentLevelSelection.Contains(_selectedLevel)) {
            // if so remove it
            currentLevelSelection.Remove(_selectedLevel);
            // change the button color
            button.GetComponent<Image>().color = deselectedColor;
        // if its not in, make sure the limit of levels selected hasnt been met
        } else if(currentLevelSelection.Count < newGame.GetNumberOfLevels) {
            // add the level
            currentLevelSelection.Add(_selectedLevel);
            // change the button color
            button.GetComponent<Image>().color = selectedColor;
        }
    }

    void ClearSelectedLevels() {
        // clear list of characters
        newGame.GetLevelSelection.Clear();
        // return each button to white
        foreach (Button button in levelButtons) {
            button.GetComponent<Image>().color = deselectedColor;
        }
    }

    public void ConfirmSelection() {
        // make sure we have the correct number of levels selected before we continue
        if (newGame.GetLevelSelection.Count == newGame.GetNumberOfLevels) {
            // load the next menu screen, whatever that may be
            mainMenu.Next(newGame);
        }
    }
}
