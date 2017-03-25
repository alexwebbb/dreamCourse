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
        foreach (GameObject levelGameObject in assetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(levelList, false);
                levelButton.GetComponentInChildren<Text>().text = level.levelName;
                Button levelButtonComponent = levelButton.GetComponent<Button>();
                // add listener for click event on buttons
                levelButtonComponent.onClick.AddListener(() => LevelSelected(level, levelButtonComponent));
                // add buttons to a list
                levelButtons.Add(levelButtonComponent);
            }
        }
    }

    void LevelSelected(Level _selectedLevel, Button button) {
        if (newGame.GetLevelSelection.Contains(_selectedLevel)) {
            newGame.GetLevelSelection.Remove(_selectedLevel);
            button.GetComponent<Image>().color = deselectedColor;
        } else if(newGame.GetLevelSelection.Count < newGame.GetNumberOfLevels) {
            newGame.GetLevelSelection.Add(_selectedLevel);
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
        if (newGame.GetLevelSelection.Count == newGame.GetNumberOfLevels) {
            // load the level
            mainMenu.Next(newGame);
        }
    }
}
