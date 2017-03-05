using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour {

    static SessionController Instance;

    LevelController levelController;
    AssetManager assetManager;

	void Start () {
		
        // singleton

        if(Instance != null) {
            GameObject.Destroy(gameObject);
        } else {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }
	}


    public void StartGame (string fileName) {

        SceneManager.LoadScene(fileName);

        // instantiate level controller
        gameObject.AddComponent<LevelController>();
        // call initialize function on level controller
        // and pass it character objects

    }

}
