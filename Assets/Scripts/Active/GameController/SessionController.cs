using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour {

    static SessionController Instance;

    LevelController levelController;
    AssetManager assetManager;

    List<Level> levels;
    int currentLevel;
    List<Character> characters;
    int numberOfPlayers;

    NewGame newGame;

    void Start() {

        // singleton

        if (Instance != null) {
            GameObject.Destroy(gameObject);
        } else {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // subscribe to on scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // get the level controller
        levelController = GetComponent<LevelController>();
    }

    public void StartGame (NewGame newGame) {

        // pass characters array and number of players
        levels = newGame.GetLevelSelection;
        characters = newGame.GetCharacterSelection;
        numberOfPlayers = newGame.GetNumberOfPlayers;
        currentLevel = 0;
        // load the requested scene
        SceneManager.LoadScene(levels[currentLevel].fileName);

        // on scene load will now run
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        Debug.Log(scene);
        Debug.Log(mode);

        levelController.Initialize(levels, currentLevel, characters, numberOfPlayers);
    }
}
