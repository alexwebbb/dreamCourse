using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Slider zoomControl;
    public VirtualJoystick joystick1;

    public float verticalScalar = 2f;
    public float horizontalScalar = 5f;
    public float defaultHeight = 5;

    LevelController levelController;
    Character activePlayer;
    GameObject player;
    Transform cameraTransform;


    void Start() {

        // subscribe to player set event
        levelController = FindObjectOfType<LevelController>();
        levelController.setActivePlayerEvent += SetActivePlayer;

        zoomControl.onValueChanged.AddListener((value) => DollyCamera(value));
    }

    void FixedUpdate() {

        if (cameraTransform != null) {
            // follows the object placed in the player position
            cameraTransform.position = player.transform.position;

            // right joystickk control. this is control for rotating the camera
            cameraTransform.Rotate(joystick1.Vertical() * verticalScalar, joystick1.Horizontal() * horizontalScalar, 0);
            // following line locks the z axis down, since Rotate is leaky. also where rotation clamp is performed
            cameraTransform.localEulerAngles = new Vector3(RotationClamp(cameraTransform.localEulerAngles.x, -30f, 50f), cameraTransform.localEulerAngles.y, 0);    
        }
    }

    void DollyCamera(float distance) {
        cameraTransform.localScale = Vector3.one * ((distance * 9.5f) + 0.5f);
    }

    void SetActivePlayer(Character _activePlayer) {
        
        // take the active player sent by the level controller and assign it locally
        Debug.Log("Howdy from the cam control: active player " + _activePlayer);
        activePlayer = _activePlayer;

        // get the player object which the camera follows
        player = activePlayer.GetPlayer;

        // get the camera transform which the camera sits inside of
        cameraTransform = activePlayer.GetCameraTransform;

    }

    float RotationClamp(float rotation, float min, float max) {

        min += 720;
        max += 720;

        min = min % 360;
        max = max % 360;

        if(min > max) {
            if (rotation < ((min - max) / 2) + max) rotation += 360;
            max += 360;
            return Mathf.Clamp(rotation, min, max);
        } else {
            return Mathf.Clamp(rotation, min, max);
        }

    }
}
