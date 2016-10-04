using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject virtualJoystick;


	void Awake () {;
        virtualJoystick.SetActive(true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
