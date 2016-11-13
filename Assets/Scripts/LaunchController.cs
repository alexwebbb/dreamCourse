using UnityEngine;
using System.Collections;

public class LaunchController : MonoBehaviour {

    public GameObject tracerObject;
    public GameObject playerObject;

    public float force;
    public Vector3 spinForce;
    public Vector3 launchDirection;
    public float lifetime;
    public float angleDrag;
    public float drag;

    public float ballGap;

    bool launchBool;
    bool playerLaunchBool;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {

            if (!launchBool) {
                StartCoroutine("BallLooper");
            } else {
                StopCoroutine("BallLooper");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) playerLaunchBool = true;
    }

    IEnumerator BallLooper() {

        launchBool = true;
        while (launchBool) {
            
            if(playerLaunchBool) {
                // launch player, end coroutine
                Launch(playerObject, false);
                playerLaunchBool = false;
                launchBool = false;

            } else {
                // launch tracer
                Launch(tracerObject, true);
            }

            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }

    void Launch(GameObject missile, bool traceBool) {

        GameObject testBall = traceBool ? (GameObject)Instantiate(missile, playerObject.transform.position, transform.rotation) : playerObject;
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        testballRB.maxAngularVelocity = 1000f;

        testballRB.AddRelativeForce(launchDirection * force, ForceMode.Impulse);
        testballRB.AddTorque(spinForce);
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        if(traceBool) Destroy(testBall, lifetime);

    }


}
