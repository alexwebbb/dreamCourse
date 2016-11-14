using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    // public Crosshairs crosshairs;
    Camera viewCamera;

    LaunchController launchCon;

    Ray ray;
    Plane groundPlane;
    float rayDistance;
    bool launchMode;

    void Start() {
        launchCon = FindObjectOfType<LaunchController>();
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
        if (launchMode) {

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
    }

    void LookAt(Vector3 lookPoint) {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
