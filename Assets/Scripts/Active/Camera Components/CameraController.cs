using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour {

    public enum CameraMode { Perspective, Orthographic }
    public CameraMode camMode = CameraMode.Orthographic;

    public Slider zoomControl;
    public Button leftButton;
    public Button rightButton;
    public VirtualJoystick joystick1;

    public float verticalScalar = 2f;
    public float horizontalScalar = 5f;
    public float defaultHeight = 5;

    enum Direction { Left, Right }

    LevelController levelController;
    Character activePlayer;
    GameObject player;
    Transform perspCamTransform;
    Transform orthoCamTransform;
    Transform orthoSubTransform;


    void Start() {

        // subscribe to player set event
        levelController = FindObjectOfType<LevelController>();
        levelController.setActivePlayerEvent += SetActivePlayer;

        zoomControl.onValueChanged.AddListener((value) => DollyCamera(value));
        leftButton.onClick.AddListener(() => RotateView(Direction.Left));
        rightButton.onClick.AddListener(() => RotateView(Direction.Right));
    }

    void FixedUpdate() {

        switch (camMode) {
            case CameraMode.Perspective:
                if (perspCamTransform != null) {
                    //
                    if (!perspCamTransform.gameObject.activeSelf) {
                        orthoCamTransform.gameObject.SetActive(false);
                        perspCamTransform.gameObject.SetActive(true);
                    }

                    // follows the object placed in the player position
                    perspCamTransform.position = player.transform.position;

                    // right joystickk control. this is control for rotating the camera
                    perspCamTransform.Rotate(joystick1.GetVertical * verticalScalar, joystick1.GetHorizontal * horizontalScalar, 0);
                    // following line locks the z axis down, since Rotate is leaky. also where rotation clamp is performed
                    perspCamTransform.localEulerAngles = new Vector3(RotationClamp(perspCamTransform.localEulerAngles.x, -30f, 50f), perspCamTransform.localEulerAngles.y, 0);
                }
                break;
            case CameraMode.Orthographic:
                if (orthoCamTransform != null) {
                    //
                    if (!orthoCamTransform.gameObject.activeSelf) {
                        perspCamTransform.gameObject.SetActive(false);
                        orthoCamTransform.gameObject.SetActive(true);
                    }

                    // follows the object placed in the player position
                    orthoCamTransform.position = player.transform.position;

                    orthoSubTransform.Translate(Vector3.forward * joystick1.GetVertical, Space.Self);
                    orthoSubTransform.Translate(Vector3.right * joystick1.GetHorizontal, Space.Self);

                }
                break;
        }
        
    }

    void DollyCamera(float distance) {
        
        switch (camMode) {
            case CameraMode.Perspective:
                perspCamTransform.localScale = Vector3.one * ((distance * 9.5f) + 0.5f);
                break;
            case CameraMode.Orthographic:
                activePlayer.GetOrthoCamera.orthographicSize = ((distance * 21f) + 9f);
                break;
        }

    }

    void RotateView(Direction direction) {
        switch (camMode) {
            case CameraMode.Orthographic:
                switch (direction) {
                    case Direction.Left:
                        orthoSubTransform.Rotate(new Vector3(0, -15, 0));
                        break;
                    case Direction.Right:
                        orthoSubTransform.Rotate(new Vector3(0, 15, 0));
                        break;
                }
                break;
        }
    }

    void SetActivePlayer(Character _activePlayer) {
        
        // take the active player sent by the level controller and assign it locally
        Debug.Log("Howdy from the cam control: active player " + _activePlayer);
        activePlayer = _activePlayer;

        // get the player object which the camera follows
        player = activePlayer.GetPlayer;

        // get the camera transform and camera for perspective camera
        perspCamTransform = activePlayer.GetPerspCamTransform;

        // get the camera transform and camera for ortho camera
        orthoCamTransform = activePlayer.GetOrthoCamTransform;
        orthoSubTransform = activePlayer.GetOrthoCamSubTransform;

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
