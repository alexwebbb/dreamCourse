using UnityEngine;
using System.Collections;

public class LaunchController : MonoBehaviour {

    public GameObject tracerObject;
    public float force;
    public Vector3 spinForce;
    public float lifetime;
    public float angleDrag;
    public float drag;

    public float ballGap;

    
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
    }

    IEnumerator BallLooper() {

        bool launchBool = true;
        while (launchBool) {
            
            if(Input.GetKeyDown(KeyCode.Space)) {
                // launch player, end coroutine
            } else {
                // launch tracer
            }

            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }

    void Launch(GameObject missile, bool traceBool) {

        GameObject testBall = (GameObject)Instantiate(missile, transform.position, transform.rotation);
        Rigidbody testballRB = testBall.GetComponent<Rigidbody>();

        testballRB.maxAngularVelocity = 1000f;

        testballRB.AddRelativeForce(new Vector3(0, 1, 0) * force, ForceMode.Impulse);
        testballRB.AddTorque(spinForce);
        testballRB.angularDrag = angleDrag;
        testballRB.drag = drag;

        if(traceBool) Destroy(testBall, lifetime);

    }


}
