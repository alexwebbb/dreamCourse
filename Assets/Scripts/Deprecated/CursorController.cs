using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public VirtualJoystick joystick1;

    public enum Mode { Mouse, Joystick };
    public Mode controlMode;

    public float scalar = 2;

    // public Crosshairs crosshairs;
    Camera viewCamera;

    LaunchControllerOld launchCon;

    Ray ray;
    Plane groundPlane;
    float rayDistance;
    bool launchMode;

    void Start() {
        launchCon = FindObjectOfType<LaunchControllerOld>();
        launchCon.launchModeBegin += StartLaunch;
        launchCon.launchModeEnd += EndLaunch;
        viewCamera = Camera.main;
    }

    void StartLaunch () {
        launchMode = true;
    }

    void EndLaunch() {
        launchMode = false;
    }

    void Update() {
        if (launchMode && controlMode == Mode.Mouse) {

            // Look Input
            ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            groundPlane = new Plane(Vector3.up, Vector3.up * transform.position.y);

            if (groundPlane.Raycast(ray, out rayDistance)) {
                Vector3 point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
                // overridden Look At
                LookAt(point);
                // crosshairs.transform.position = point;
                // crosshairs.DetectTargets(ray);
            } 
        }

        if (launchMode && controlMode == Mode.Joystick) {
            transform.Rotate(joystick1.Vertical() * scalar, joystick1.Horizontal() * scalar, 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

    }

    void LookAt(Vector3 lookPoint) {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
