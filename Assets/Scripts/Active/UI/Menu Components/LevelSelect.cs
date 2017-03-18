using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MenuComponent, ISelectionMenu {

    public GameObject levelListButton;

    Transform levelList;
    //should be private
    public NewGame newGame;

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
        newGame.ClearLevelSelection();
    }




    void InitializeLevelList() {
        foreach (GameObject levelGameObject in assetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(levelList, false);
                levelButton.GetComponentInChildren<Text>().text = level.levelName;
                Button levelButtonComponent = levelButton.GetComponent<Button>();

                levelButtonComponent.onClick.AddListener(() => LevelSelected(level, levelButtonComponent));
            }
        }
    }

    void LevelSelected(Level _selectedLevel, Button button) {
        if (newGame.GetLevelSelection.Contains(_selectedLevel)) {
            newGame.GetLevelSelection.Remove(_selectedLevel);
            button.GetComponent<Image>().color = Color.white;
        } else {
            newGame.GetLevelSelection.Add(_selectedLevel);
            button.GetComponent<Image>().color = Color.green;

            if (newGame.GetLevelSelection.Count >= newGame.GetNumberOfLevels) {
                Debug.Log("Load");

                mainMenu.Next(newGame);
            }
        }
    }
}
