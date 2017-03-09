using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Character activePlayer;
    public VirtualJoystick joystick1;

    LevelController levelController;
    public GameObject player;
    Transform cameraTransform;
    public float scalar = 0.1f;
    public float defaultHeight = 5;

    void Start() {

        // subscribe to player set event
        levelController = FindObjectOfType<LevelController>();
        levelController.setActivePlayerEvent += SetActivePlayer;
        
        // ensures that the camera is pointing in the same direction as the player at the start.
        // cameraTransform.forward = player.transform.parent.forward;

    }

    void FixedUpdate() {

        if (cameraTransform != null) {
            // follows the object placed in the player position
            cameraTransform.position = player.transform.position;

            // right joystickk control. this is control for rotating the camera
            cameraTransform.Rotate(joystick1.Vertical(), joystick1.Horizontal(), 0);
            cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x, cameraTransform.localRotation.eulerAngles.y, 0);

            // zooming in and out by scaling camera container
            cameraTransform.localScale += new Vector3(joystick1.Vertical() * scalar, joystick1.Vertical() * scalar, joystick1.Vertical() * scalar);

            // Clamps range of zoom
            cameraTransform.localScale = new Vector3(Mathf.Clamp(cameraTransform.localScale.x, 0.5f, 10f), Mathf.Clamp(cameraTransform.localScale.y, 0.5f, 10f), Mathf.Clamp(cameraTransform.localScale.z, 0.5f, 10f));    
        }
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
}
