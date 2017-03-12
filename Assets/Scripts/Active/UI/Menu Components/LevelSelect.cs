using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour, ISelectionMenu {

    public GameObject levelListButton;
    public GameObject levelList;

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

    MainMenu mainMenu;
    GameObject elements;
    AssetManager assetManager;
    NewGame newGame;

    void Start() {
        InitializeLevelList();
    }

    void InitializeLevelList() {
        foreach (GameObject levelGameObject in GetAssetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(levelList.transform, false);
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
            }
        }
    }
}
