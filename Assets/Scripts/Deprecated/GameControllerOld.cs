using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject cameraContainer;
    public GameObject virtualJoystick;

    public bool joystickOn;

	void Awake () {

        if (joystickOn) {
            virtualJoystick.SetActive(true);
        } else {
            cameraContainer.SetActive(false);
        }
	
	}
	
}
