using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {

    public GameObject levelListButton;
    public GameObject characterListButton;
    [Space(10)]
    public GameObject canvas;
    public GameObject mainPanel;
    [Space(10)]
    [Header("Level Select Menu")]
    public GameObject levelSelect;
    public GameObject levelSelectElements;
    public GameObject levelList;
    [Space(10)]
    [Header("Character Select Menu")]
    public GameObject characterSelect;
    public GameObject characterSelectElements;
    public GameObject characterList;
    [Space(10)]
    [Header("Confirmation Menu")]
    public GameObject confirmSelect;
    public GameObject confirmSelectElements;
    [Space(10)]

    public GameObject launchUI;


    Level selectedLevel;
    List<GameObject> selectedCharacter = new List<GameObject>();
    int numberOfPlayers;

    SessionController sessionController;
    AssetManager assetManager;

	void Start () {

        // load session controller, asset manager
        sessionController = GetComponent<SessionController>();
        assetManager = GetComponent<AssetManager>();

        InitializeLevelList();
        InitializeCharacterList();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void NewGameButton() {
        Debug.Log("newgame clicked");

        // animate first layer to a little block in the corner
        // deactivate buttons

        mainPanel.SetActive(false);
        levelSelectElements.SetActive(true);


        // set as not interactable
    }

    public void BackToMainMenu() {
        levelSelectElements.SetActive(false);
        mainPanel.SetActive(true);
    }

    
    public void LevelLoadTest() {
        canvas.SetActive(false);
        launchUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        sessionController.StartGame(selectedLevel.fileName, selectedCharacter, numberOfPlayers);

    }
    



    void LevelSelected(Level _selectedLevel) {
        selectedLevel = _selectedLevel;
        levelSelect.SetActive(false);
        characterSelectElements.SetActive(true);
    }

    void CharacterSelected(Character _selectedCharacter) {
        // this may need to be an ienumerator
        selectedCharacter.Add(_selectedCharacter.gameObject);
        Debug.Log(selectedCharacter.Count);
        if(selectedCharacter.Count >= numberOfPlayers) {
            characterSelect.SetActive(false);
            confirmSelectElements.SetActive(true);
        }        
    }

    // these need to be set up so they call a tertiary function the makes them not interactable
    public void OnePlayer () { numberOfPlayers = 1; }
    public void TwoPlayer () { numberOfPlayers = 2; }

    void InitializeLevelList() {
        foreach(GameObject levelGameObject in assetManager.level) {
            if (levelGameObject != null) {
                Level level = levelGameObject.GetComponent<Level>();

                GameObject levelButton = Instantiate<GameObject>(levelListButton);
                levelButton.transform.SetParent(levelList.transform, false);
                levelButton.GetComponentInChildren<Text>().text = level.levelName;
                
                levelButton.GetComponent<Button>().onClick.AddListener(() => LevelSelected(level)); 
                
            }
        }
    }

    void InitializeCharacterList() {
        foreach (GameObject characterGameObject in assetManager.character) {
            if (characterGameObject != null) {
                Character character = characterGameObject.GetComponent<Character>();

                GameObject characterButton = Instantiate<GameObject>(characterListButton);
                characterButton.transform.SetParent(characterList.transform, false);
                characterButton.GetComponentInChildren<Text>().text = character.characterName;

                characterButton.GetComponent<Button>().onClick.AddListener(() => CharacterSelected(character));
            }
        }
    }
}
