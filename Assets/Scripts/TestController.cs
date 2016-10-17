using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

    public GameObject launchedObject;
    public float force;
    public Vector3 spinForce;
    public float lifetime;
    public float angleDrag;

    public float ballGap;

    bool launchBool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L)) {

            if(!launchBool) {
                StartCoroutine("BallLooper");
            } else {
                StopCoroutine("BallLooper");
            }
        }
	}

    IEnumerator BallLooper() {

        launchBool = true;
        while(launchBool) {
            GameObject testBall = (GameObject)Instantiate(launchedObject, transform.position, transform.rotation);
            Rigidbody testballRB = testBall.GetComponent<Rigidbody>();
            testballRB.AddRelativeForce(new Vector3(0, 1, 0) * force, ForceMode.Impulse);
            testballRB.AddTorque(spinForce);
            testballRB.angularDrag = angleDrag;
            Destroy(testBall, lifetime);
            yield return new WaitForSeconds(ballGap);
        }

        yield return null;
    }
}
