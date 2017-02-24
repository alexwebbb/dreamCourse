using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour {

    static SessionController Instance;

    enum Mode {
        Test006 = 0,
        Test007 = 1
    }

	void Start () {
		
        if(Instance != null) {
            GameObject.Destroy(gameObject);
        } else {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            StartGame(Mode.Test006);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            StartGame(Mode.Test007);
        }
	}



    void StartGame (Mode mode) {


        if (Mode.Test006.Equals(mode)) {
            Debug.Log("Test1");
            SceneManager.LoadScene("test006");
            
        } else if (Mode.Test007.Equals(mode)) {
            Debug.Log("Test2");
            SceneManager.LoadScene("test007");

        }

    }
}
