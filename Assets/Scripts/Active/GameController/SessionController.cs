using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour {

    static SessionController Instance;

    LevelController levelController;
    AssetManager assetManager;

    GameObject[] characters;

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

    public void StartGame (string fileName, GameObject[] _characters) {

        // load the requested scene
        SceneManager.LoadScene(fileName);

        // pass characters array
        characters = _characters;

        // on scene load will now run
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        Debug.Log(scene);
        Debug.Log(mode);

        levelController.Initialize(characters, 1);
    }
}
