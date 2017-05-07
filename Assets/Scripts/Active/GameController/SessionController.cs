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
        assetManager = GetComponent<AssetManager>();
    }

    public void StartGame (NewGame newGame) {

        // pass characters array and number of players
        characters = newGame.GetCharacterSelection;
        numberOfPlayers = newGame.GetNumberOfPlayers;

        // optional elements, probably will be replaced w/ game object later
        levels = newGame.GetLevelSelection;
        currentLevel = 0;

        // load the requested scene
        SceneManager.LoadScene(levels[currentLevel].fileName);
        // should add the current level to a list
        // whenever the player moves to a new area, it calls a function like NextLevel
        // which checks to see if the requested level has already been visited, and if so
        // supplies that prior version which has had its variables modified
        // actually may store as strings and integers

        // on scene load will now run
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        Debug.Log(scene);
        Debug.Log(mode);

        levelController.Initialize(characters, numberOfPlayers, assetManager.cameras);
    }
}
