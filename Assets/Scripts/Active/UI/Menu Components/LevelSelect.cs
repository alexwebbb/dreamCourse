using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour, ISelectionMenu {

    public GameObject levelListButton;

    public MainMenu GetMainMenu {
        get {
            if (mainMenu == null) mainMenu = transform.GetComponentInParent<MainMenu>();
            return mainMenu;
        }
    }

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.FindChild("Elements").gameObject;
            return elements;
        }
    }

    public AssetManager GetAssetManager {
        get {
            if (assetManager == null) assetManager = transform.GetComponentInParent<AssetManager>();
            return assetManager;
        }
    }

    public Transform GetLevelList {
        get {
            if (levelList == null) levelList = transform.Find("Elements/Level List Mask/Level List");
            return levelList;
        }
    }

    MainMenu mainMenu;
    GameObject elements;
    AssetManager assetManager;
    Transform levelList;
    public NewGame newGame;

    void Start() {
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
        foreach (GameObject levelGameObject in GetAssetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(GetLevelList, false);
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

                GetMainMenu.Next(newGame);
            }
        }
    }
}
